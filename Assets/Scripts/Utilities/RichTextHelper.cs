using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public static class RichTextHelper
{
    public static string ReplaceValues(string input, float attackValue)
    {
        return Regex.Replace(input, @"\{([^}]+)\}", match =>
        {
            string expr = match.Groups[1].Value.Trim();

            // 1. 处理 {攻击力}
            if (expr == "攻击力")
            {
                return attackValue.ToString("0.##");
            }

            // 2. 处理 {攻击力*1.1}
            if (expr.StartsWith("攻击力*"))
            {
                string factorStr = expr.Substring("攻击力*".Length);

                if (float.TryParse(factorStr, out float factor))
                {
                    float result = attackValue * factor;
                    //return result.ToString("0.##");
                    return Mathf.CeilToInt(result).ToString();
                }
            }
            //if (expr.StartsWith("攻击力*"))
            //{
            //    string factorStr = expr.Substring("攻击力*".Length);

            //    if (float.TryParse(factorStr, out float factor))
            //    {
            //        float result = attackValue * factor;
            //        //return result.ToString("0.##");
            //        return Mathf.CeilToInt(result).ToString();
            //    }
            //}
            // 未来可扩展更多表达式
            return match.Value;
        });
    }
}
