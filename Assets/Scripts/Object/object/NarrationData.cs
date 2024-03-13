using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrationData
{
    private Dictionary<string, string[]> narreationTexts;
    private List<string> narreationKoreanTexts;
    private List<string> narreationEnglishTexts;
    public List<string> NarreationKoreanTexts => narreationKoreanTexts;
    public List<string> NarreationEnglishTexts => narreationEnglishTexts;
    string z;
    public NarrationData()
    {
        narreationTexts = Main.Data.CSVLengthCutting(Main.Data.CSVLoader(Define.CSV.LanguagePack));
        narreationKoreanTexts = new List<string>(narreationTexts["Korean"]);
        narreationEnglishTexts = new List<string>(narreationTexts["English"]);
    }
    public List<string> GetNarreationTexts(int value)
    {
        List<string> Texts;

        if (value ==0)
        {
            Texts = narreationEnglishTexts;
        }
        else
        {
            Texts = narreationKoreanTexts;
        }

        return Texts;
    }
}
