using UnityEngine;

namespace AudioManipulation
{
    public struct AudioUnitConversions
    {
        public static float DBToRel(float dB)
        {
            return Mathf.Pow(10, dB / 20);
        }

        public static float RelToDB(float rel)
        {
            return 20 * Mathf.Log10(rel);
        }

        public static float[] DBsToRels(float[] dBs)
        {
            float[] rels = new float[dBs.Length];

            for (int i = 0; i < rels.Length; i++)
            {
                rels[i] = DBToRel(dBs[i]);
            }

            return rels;
        }

        public static float[] RelsToDBs(float[] rels)
        {
            float[] dBs = new float[rels.Length];

            for (int i = 0; i < dBs.Length; i++)
            {
                dBs[i] = RelToDB(rels[i]);
            }

            return dBs;
        }
    }

    public struct AudioUnitOperations
    {
        public static float AddDBs(float dB1, float dB2)
        {
            return AddDBs(new float[2] { dB1, dB2 });
        }

        public static float AddDBs(float[] dBs)
        {
            float sum = 0;

            for (int i = 0; i < dBs.Length; i++)
            {
                sum += Mathf.Pow(10, dBs[i] / 10);
            }

            return 10 * Mathf.Log10(sum);
        }

        public static float AddRels(float rel1, float rel2)
        {
            return AddRels(new float[] { rel1, rel2 });
        }

        public static float AddRels(float[] rels)
        {
            return AudioUnitConversions.DBToRel(AddDBs(AudioUnitConversions.RelsToDBs(rels)));
        }
    }
}
