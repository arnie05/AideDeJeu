﻿using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace AideDeJeuLib.Spells
{
    public class Spell : Item
    {
        public string LevelType
        {
            get
            {
                if (int.Parse(Level) > 0)
                {
                    if (string.IsNullOrEmpty(Rituel))
                    {
                        return $"{Type} de niveau {Level}";
                    }
                    else
                    {
                        return $"{Type} de niveau {Level} {Rituel}";
                    }
                }
                else
                {
                    return $"{Type}, tour de magie";
                }
            }
            set
            {
                var re = new Regex("(?<type>.*) de niveau (?<level>\\d).?(?<rituel>\\(rituel\\))?");
                var match = re.Match(value);
                this.Type = match.Groups["type"].Value;
                this.Level = match.Groups["level"].Value;
                this.Rituel = match.Groups["rituel"].Value;
                if (string.IsNullOrEmpty(this.Type))
                {
                    re = new Regex("(?<type>.*), (?<level>tour de magie)");
                    match = re.Match(value);
                    if (match.Groups["level"].Value == "tour de magie")
                    {
                        this.Type = match.Groups["type"].Value;
                        this.Level = "0"; // match.Groups["level"].Value;
                        this.Rituel = match.Groups["rituel"].Value;
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine(value);
                        //re = new Regex("level (?<level>\\d) - (?<type>.*?)\\w?(?<rituel>\\(ritual\\))?");
                        re = new Regex("^(?<level>\\d) - (?<type>.*?)\\s?(?<rituel>\\(ritual\\))?$");
                        match = re.Match(value);
                        this.Type = match.Groups["type"].Value;
                        this.Level = match.Groups["level"].Value;
                        this.Rituel = match.Groups["rituel"].Value;
                    }
                }
            }
        }
        public string Level { get; set; }
        public string Type { get; set; }
        public string Concentration { get; set; }
        public string Rituel { get; set; }
        public string CastingTime { get; set; }
        public string Range { get; set; }
        public string Components { get; set; }
        public string Duration { get; set; }
        public string DescriptionHtml { get; set; }

        public string Source { get; set; }

        public override string Markdown
        {
            get
            {
                return
                    $"# {Name}\n" +
                    $"{NameVO}\n" +
                    $"_{LevelType}_\n" +
                    $"**Temps d'incantation :** {CastingTime}\n" +
                    $"**Portée :** {Range}\n" +
                    $"**Composantes :** {Components}\n" +
                    $"**Durée :** {Duration}\n\n" +
                    $"{DescriptionHtml}\n\n" +
                    $"**Source :** {Source}";

            }
        }
    }
}
