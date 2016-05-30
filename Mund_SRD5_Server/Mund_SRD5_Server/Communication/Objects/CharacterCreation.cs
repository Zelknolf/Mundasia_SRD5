using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Mundasia.Objects;

namespace Mundasia.Communication
{
    public class CharacterCreation
    {
        // We use a printable character as a delimiter to make sure that the end format is something simple and
        // describable.
        private static string delimiter = "|";
        private static char[] delim = new char[] { '|' };

        private static string listDelimiter = "[";
        private static char[] listDelim = new char[] { '[' };

        public string Name;
        public int Gender;
        public Race Race;
        public CharacterClass Class;
        public Background Background;
        public Alignment Alignment;
        public List<Skill> RaceSkills;
        public List<Skill> ClassSkills;
        public List<Skill> ClassTools;
        public List<Spell> Cantrips;
        public List<Spell> SpellsKnown;
        public int BaseStrength;
        public int BaseDexterity;
        public int BaseConstitution;
        public int BaseIntelligence;
        public int BaseWisdom;
        public int BaseCharisma;

        public int HairStyle;
        public int HairColor;
        public int SkinColor;

        public string UserId;
        public int SessionId;

        public bool Valid;

        public CharacterCreation() { }

        public CharacterCreation(string buildLine)
        {
            Valid = false;
            string[] buildSplit = buildLine.Split(delim);
            if(buildSplit.Length != 22) { return; }
            Name = buildSplit[0];

            if(!int.TryParse(buildSplit[1], out Gender))
            {
                return;
            }

            uint tempUint = 0;
            if(!uint.TryParse(buildSplit[2], out tempUint))
            {
                return;
            }

            Race = Race.GetRace(tempUint);
            if(Race == null)
            {
                return;
            }

            if(!uint.TryParse(buildSplit[3], out tempUint))
            {
                return;
            }

            Class = CharacterClass.GetClass(tempUint);
            if(Class == null)
            {
                return;
            }

            if(!uint.TryParse(buildSplit[4], out tempUint))
            {
                return;
            }

            Background = Background.GetBackground(tempUint);
            if(Background == null)
            {
                return;
            }

            if(!uint.TryParse(buildSplit[5], out tempUint))
            {
                return;
            }

            Alignment = Alignment.GetAlignment(tempUint);
            if(Alignment == null)
            {
                return;
            }

            RaceSkills = new List<Skill>();
            string[] raceSkillSplit = buildSplit[6].Split(listDelim);
            foreach(string raceSkill in raceSkillSplit)
            {
                if(String.IsNullOrWhiteSpace(raceSkill))
                {
                    continue;
                }

                uint skillId = 0;
                if(!uint.TryParse(raceSkill, out skillId))
                {
                    return;
                }
                Skill toAdd = Skill.GetSkill(skillId);
                if(toAdd == null)
                {
                    return;
                }
                RaceSkills.Add(toAdd);
            }
            if(RaceSkills.Count > Race.FreeSkills)
            {
                return;
            }
            foreach(Skill sk in RaceSkills)
            {
                if(!Race.SelectedSkill.Contains(sk))
                {
                    return;
                }
            }

            ClassSkills = new List<Skill>();
            string[] classSkillSplit = buildSplit[7].Split(listDelim);
            foreach(string classSkill in classSkillSplit)
            {
                if(String.IsNullOrWhiteSpace(classSkill))
                {
                    continue;
                }

                uint skillId = 0;
                if(!uint.TryParse(classSkill, out skillId))
                {
                    return;
                }
                Skill toAdd = Skill.GetSkill(skillId);
                if(toAdd == null)
                {
                    return;
                }
                ClassSkills.Add(toAdd);
            }
            if(ClassSkills.Count > Class.SkillChoices)
            {
                return;
            }
            foreach(Skill sk in ClassSkills)
            {
                if (!Class.AvailableSkills.Contains(sk))
                {
                    return;
                }
            }

            ClassTools = new List<Skill>();
            string[] classToolSplit = buildSplit[8].Split(listDelim);
            foreach(string classTool in classToolSplit)
            {
                if(String.IsNullOrWhiteSpace(classTool))
                {
                    continue;
                }

                uint skillId = 0;
                if(!uint.TryParse(classTool, out skillId))
                {
                    return;
                }
                Skill toAdd = Skill.GetSkill(skillId);
                if(toAdd == null)
                {
                    return;
                }
                ClassTools.Add(toAdd);
            }
            if(ClassTools.Count > Class.ToolChoices)
            {
                return;
            }
            foreach(Skill sk in ClassTools)
            {
                if(!Class.AvailableTools.Contains(sk))
                {
                    return;
                }
            }

            if(Class.CantripsKnown != null &&
               Class.CantripsKnown.Count > 0)
            {
                Cantrips = new List<Spell>();
                string[] cantripsSplit = buildSplit[9].Split(listDelim);
                foreach(string cantrip in cantripsSplit)
                {
                    if(String.IsNullOrWhiteSpace(cantrip))
                    {
                        continue;
                    }

                    uint spellId = 0;
                    if(!uint.TryParse(cantrip, out spellId))
                    {
                        return;
                    }
                    Spell toAdd = Spell.GetSpell(spellId);
                    if(toAdd == null)
                    {
                        return;
                    }
                    Cantrips.Add(toAdd);
                }
                if(Cantrips.Count > Class.CantripsKnown[0])
                {
                    return;
                }
                if (Cantrips.Count > 0)
                {
                    if (!Class.SpellList.ContainsKey(0))
                    {
                        return;
                    }
                    foreach (Spell sp in Cantrips)
                    {
                        if(!Class.SpellList[0].Contains(sp))
                        {
                            return;
                        }
                    }
                }
            }

            if (Class.SpellsKnown != null &&
                Class.SpellsKnown.Count > 0)
            {
                SpellsKnown = new List<Spell>();

                string[] spellSplit = buildSplit[10].Split(listDelim);
                foreach(string spellKnown in spellSplit)
                {
                    if(String.IsNullOrWhiteSpace(spellKnown))
                    {
                        continue;
                    }
                    uint spellId = 0;
                    if(!uint.TryParse(spellKnown, out spellId))
                    {
                        return;
                    }
                    Spell toAdd = Spell.GetSpell(spellId);
                    if(toAdd == null)
                    {
                        return;
                    }
                    SpellsKnown.Add(toAdd);
                }
                if(SpellsKnown.Count > Class.SpellsKnown[0])
                {
                    return;
                }
                if(SpellsKnown.Count > 0)
                {
                    if(!Class.SpellList.ContainsKey(1))
                    {
                        return;
                    }
                    foreach(Spell sp in SpellsKnown)
                    {
                        if(!Class.SpellList[1].Contains(sp))
                        {
                            return;
                        }
                    }
                }
            }

            if(!int.TryParse(buildSplit[11], out BaseStrength))
            {
                return;
            }
            if(!AbilityScoreCosts.ContainsKey(BaseStrength))
            {
                return;
            }
            if(!int.TryParse(buildSplit[12], out BaseDexterity))
            {
                return;
            }
            if(!AbilityScoreCosts.ContainsKey(BaseDexterity))
            {
                return;
            }
            if(!int.TryParse(buildSplit[13], out BaseConstitution))
            {
                return;
            }
            if(!AbilityScoreCosts.ContainsKey(BaseConstitution))
            {
                return;
            }
            if(!int.TryParse(buildSplit[14], out BaseIntelligence))
            {
                return;
            }
            if(!AbilityScoreCosts.ContainsKey(BaseIntelligence))
            {
                return;
            }
            if(!int.TryParse(buildSplit[15], out BaseWisdom))
            {
                return;
            }
            if(!AbilityScoreCosts.ContainsKey(BaseWisdom))
            {
                return;
            }
            if(!int.TryParse(buildSplit[16], out BaseCharisma))
            {
                return;
            }
            if(!AbilityScoreCosts.ContainsKey(BaseCharisma))
            {
                return;
            }

            if(GetRemainingPoints(BaseStrength, BaseDexterity, BaseConstitution, BaseIntelligence, BaseWisdom, BaseCharisma, Race) < 0)
            {
                return;
            }

            if(!int.TryParse(buildSplit[17], out HairStyle))
            {
                return;
            }
            if(!int.TryParse(buildSplit[18], out HairColor))
            {
                return;
            }
            if(!int.TryParse(buildSplit[19], out SkinColor))
            {
                return;
            }
            UserId = buildSplit[20];

            if(!int.TryParse(buildSplit[21], out SessionId))
            {
                return;
            }

            Valid = true;
        }

        public override string ToString()
        {
            return String.Empty;
        }


        public static int GetRemainingPoints(int str, int dex, int con, int intel, int wis, int cha, Race race)
        {
            int _strengthCostAdjust = 0;
            int _dexterityCostAdjust = 0;
            int _constitutionCostAdjust = 0;
            int _wisdomCostAdjust = 0;
            int _intelligenceCostAdjust = 0;
            int _charismaCostAdjust = 0;
            if (race != null &&
                race.OtherBonusStats > 0)
            {
                int bonuses = race.OtherBonusStats;
                List<int> scores = new List<int>();
                if (race.Strength == 0) scores.Add(str);
                if (race.Dexterity == 0) scores.Add(dex);
                if (race.Constitution == 0) scores.Add(con);
                if (race.Intelligence == 0) scores.Add(intel);
                if (race.Wisdom == 0) scores.Add(wis);
                if (race.Charisma == 0) scores.Add(cha);

                scores.Sort();
                while (bonuses > 0)
                {
                    if (str == scores[scores.Count - bonuses] &&
                        _strengthCostAdjust == 0)
                    {
                        _strengthCostAdjust--;
                    }
                    else if (dex == scores[scores.Count - bonuses] &&
                        _dexterityCostAdjust == 0)
                    {
                        _dexterityCostAdjust--;
                    }
                    else if (con == scores[scores.Count - bonuses] &&
                        _constitutionCostAdjust == 0)
                    {
                        _constitutionCostAdjust--;
                    }
                    else if (intel == scores[scores.Count - bonuses] &&
                        _intelligenceCostAdjust == 0)
                    {
                        _intelligenceCostAdjust--;
                    }
                    else if (wis == scores[scores.Count - bonuses] &&
                        _wisdomCostAdjust == 0)
                    {
                        _wisdomCostAdjust--;
                    }
                    else if (cha == scores[scores.Count - bonuses] &&
                        _charismaCostAdjust == 0)
                    {
                        _charismaCostAdjust--;
                    }
                    bonuses--;
                }
            }


            return 40 - AbilityScoreCosts[str + _strengthCostAdjust] -
                        AbilityScoreCosts[dex + _dexterityCostAdjust] -
                        AbilityScoreCosts[con + _constitutionCostAdjust] -
                        AbilityScoreCosts[intel + _intelligenceCostAdjust] -
                        AbilityScoreCosts[wis + _wisdomCostAdjust] -
                        AbilityScoreCosts[cha + _charismaCostAdjust];
        }

        private static Dictionary<int, int> AbilityScoreCosts = new Dictionary<int, int>()
        {
            { 0, 0 },
            { 1, 0 },
            { 2, 0 },
            { 3, 0 },
            { 4, 0 },
            { 5, 0 },
            { 6, 0 },
            { 7, 1 },
            { 8, 2 },
            { 9, 3 },
            { 10, 4 },
            { 11, 5 },
            { 12, 6 },
            { 13, 8 },
            { 14, 10 },
            { 15, 12 },
            { 16, 14 },
        };
    }
}
