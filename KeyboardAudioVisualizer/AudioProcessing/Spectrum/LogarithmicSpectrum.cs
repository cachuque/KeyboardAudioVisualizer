﻿using System;
using KeyboardAudioVisualizer.Helper;

namespace KeyboardAudioVisualizer.AudioProcessing.Spectrum
{
    public class LogarithmicSpectrum : AbstractSpectrum
    {
        #region Constructors

        public LogarithmicSpectrum(float[] data, int bands, float minFrequency = -1, float maxFrequency = -1)
        {
            int dataReferenceCount = (data.Length - 1) * 2;

            int fromIndex = minFrequency < 0 ? 0 : MathHelper.Clamp(FrequencyHelper.GetIndexOfFrequency(minFrequency, dataReferenceCount), 0, data.Length - 1 - bands); // -bands since we need at least enough data to get our bands
            int toIndex = maxFrequency < 0 ? data.Length - 1 : MathHelper.Clamp(FrequencyHelper.GetIndexOfFrequency(maxFrequency, dataReferenceCount), fromIndex, data.Length - 1);

            int usableSourceData = Math.Max(bands, (toIndex - fromIndex) + 1);

            Bands = new Band[bands];

            double ratio = Math.Pow(usableSourceData, 1.0 / bands);
            double calculation = 1;

            int index = fromIndex;
            for (int i = 0; i < Bands.Length; i++)
            {
                calculation *= ratio;
                int count = Math.Max(1, ((int)calculation) - index);

                float[] bandData = new float[count];
                Array.Copy(data, index, bandData, 0, count);
                Bands[i] = new Band(FrequencyHelper.GetFrequencyOfIndex(index, dataReferenceCount),
                                    FrequencyHelper.GetFrequencyOfIndex(index + count, dataReferenceCount),
                                    bandData);

                index += count;
            }
        }

        #endregion
    }
}
