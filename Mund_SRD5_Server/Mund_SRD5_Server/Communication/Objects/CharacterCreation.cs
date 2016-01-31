using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mundasia.Communication
{
    public class CharacterCreation
    {
        // We use a printable character as a delimiter to make sure that the end format is something simple and
        // describable.
        private static string delimiter = "|";
        private static char[] delim = new char[] { '|' };

        public string Name;
        public int Sex;
        public int Race;
        public int Virtue;
        public int Vice;
        public int Authority;
        public int Care;
        public int Fairness;
        public int Loyalty;
        public int Tradition;
        public int Profession;
        public int Talent;
        public int Hobby;
        public int Aspiration;

        public int HairStyle;
        public int HairColor;
        public int SkinColor;

        public string UserId;
        public int SessionId;

        public CharacterCreation() { }

        public CharacterCreation(string buildLine)
        {
            string[] input = buildLine.Split(delim);
            Name = input[0];
            int.TryParse(input[1], out Sex);
            int.TryParse(input[2], out Race);
            int.TryParse(input[3], out Virtue);
            int.TryParse(input[4], out Vice);
            int.TryParse(input[5], out Authority);
            int.TryParse(input[6], out Care);
            int.TryParse(input[7], out Fairness);
            int.TryParse(input[8], out Loyalty);
            int.TryParse(input[9], out Tradition);
            int.TryParse(input[10], out Profession);
            int.TryParse(input[11], out Talent);
            int.TryParse(input[12], out Hobby);

            UserId = input[13];
            int.TryParse(input[14], out SessionId);
            int.TryParse(input[15], out Aspiration);
            int.TryParse(input[16], out HairStyle);
            int.TryParse(input[17], out HairColor);
            int.TryParse(input[18], out SkinColor);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Name);
            sb.Append(delimiter);
            sb.Append(Sex);
            sb.Append(delimiter);
            sb.Append(Race);
            sb.Append(delimiter);
            sb.Append(Virtue);
            sb.Append(delimiter);
            sb.Append(Vice);
            sb.Append(delimiter);
            sb.Append(Authority);
            sb.Append(delimiter);
            sb.Append(Care);
            sb.Append(delimiter);
            sb.Append(Fairness);
            sb.Append(delimiter);
            sb.Append(Loyalty);
            sb.Append(delimiter);
            sb.Append(Tradition);
            sb.Append(delimiter);
            sb.Append(Profession);
            sb.Append(delimiter);
            sb.Append(Talent);
            sb.Append(delimiter);
            sb.Append(Hobby); 
            sb.Append(delimiter);
            sb.Append(UserId);
            sb.Append(delimiter);
            sb.Append(SessionId);
            sb.Append(delimiter);
            sb.Append(Aspiration);
            sb.Append(delimiter);
            sb.Append(HairStyle);
            sb.Append(delimiter);
            sb.Append(HairColor);
            sb.Append(delimiter);
            sb.Append(SkinColor);
            return sb.ToString();
        }
    }
}
