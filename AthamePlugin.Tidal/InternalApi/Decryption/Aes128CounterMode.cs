// The MIT License (MIT)

// Copyright (c) 2014 Hans Wolff

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace AthamePlugin.Tidal.InternalApi.Decryption
{
    internal class Aes128CounterMode : SymmetricAlgorithm
    {
        private readonly byte[] counter;
        private readonly AesManaged aes;

        public Aes128CounterMode(byte[] counter)
        {
            if (counter == null) throw new ArgumentNullException(nameof(counter));


            aes = new AesManaged
            {
                Mode = CipherMode.ECB,
                Padding = PaddingMode.None
            };

            this.counter = counter;
            if (counter.Length != aes.BlockSize / 8)
                throw new ArgumentException(
                    $"Counter size must be same as block size (actual: {counter.Length}, expected: {aes.BlockSize / 8})");
        }

        public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] ignoredParameter)
        {
            return new CounterModeCryptoTransform(aes, rgbKey, counter);
        }

        public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] ignoredParameter)
        {
            return new CounterModeCryptoTransform(aes, rgbKey, counter);
        }

        public override void GenerateKey()
        {
            aes.GenerateKey();
        }

        public override void GenerateIV()
        {
            // IV not needed in Counter Mode
        }
    }

    public class CounterModeCryptoTransform : ICryptoTransform
    {
        private readonly byte[] counter;
        private readonly ICryptoTransform counterEncryptor;
        private readonly Queue<byte> xorMask = new Queue<byte>();
        private readonly SymmetricAlgorithm symmetricAlgorithm;

        public CounterModeCryptoTransform(SymmetricAlgorithm symmetricAlgorithm, byte[] key, byte[] counter)
        {
            if (symmetricAlgorithm == null) throw new ArgumentNullException(nameof(symmetricAlgorithm));
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (counter == null) throw new ArgumentNullException(nameof(counter));
            if (counter.Length != symmetricAlgorithm.BlockSize / 8)
                throw new ArgumentException(
                    $"Counter size must be same as block size (actual: {counter.Length}, expected: {symmetricAlgorithm.BlockSize / 8})");

            this.symmetricAlgorithm = symmetricAlgorithm;
            this.counter = counter;

            var zeroIv = new byte[this.symmetricAlgorithm.BlockSize / 8];
            counterEncryptor = symmetricAlgorithm.CreateEncryptor(key, zeroIv);
        }

        public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            var output = new byte[inputCount];
            TransformBlock(inputBuffer, inputOffset, inputCount, output, 0);
            return output;
        }

        public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            for (var i = 0; i < inputCount; i++)
            {
                if (NeedMoreXorMaskBytes()) EncryptCounterThenIncrement();

                var mask = xorMask.Dequeue();
                outputBuffer[outputOffset + i] = (byte)(inputBuffer[inputOffset + i] ^ mask);
            }

            return inputCount;
        }

        private bool NeedMoreXorMaskBytes()
        {
            return xorMask.Count == 0;
        }

        private void EncryptCounterThenIncrement()
        {
            var counterModeBlock = new byte[symmetricAlgorithm.BlockSize / 8];

            counterEncryptor.TransformBlock(counter, 0, counter.Length, counterModeBlock, 0);
            IncrementCounter();

            foreach (var b in counterModeBlock)
            {
                xorMask.Enqueue(b);
            }
        }

        private void IncrementCounter()
        {
            for (var i = counter.Length - 1; i >= 0; i--)
            {
                if (++counter[i] != 0)
                    break;
            }
        }

        public int InputBlockSize => symmetricAlgorithm.BlockSize / 8;
        public int OutputBlockSize => symmetricAlgorithm.BlockSize / 8;
        public bool CanTransformMultipleBlocks => true;
        public bool CanReuseTransform => false;

        public void Dispose()
        {
        }
    }
}