﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using AideDeJeu.Tools;
using Markdig.Syntax;

namespace AideDeJeuLib
{
    public class Condition : Item
    {
        public string Text { get; set; }

        public override string Markdown
        {
            get
            {
                return 
                    $"# {Name}\n\n" +
                    $"{NameVO}\n\n" +
                    Text;
            }
        }

        public override void Parse(ref ContainerBlock.Enumerator enumerator)
        {
            enumerator.MoveNext();
            while (enumerator.Current != null)
            {
                var block = enumerator.Current;
                //DumpBlock(block);
                if (block is Markdig.Syntax.HeadingBlock)
                {
                    var headingBlock = block as Markdig.Syntax.HeadingBlock;
                    //DumpHeadingBlock(headingBlock);
                    if (headingBlock.HeaderChar == '#' && (headingBlock.Level == 1 || headingBlock.Level == 2))
                    {
                        if (this.Name != null)
                        {
                            return;
                            //yield return spell;
                        }
                        this.Name = headingBlock.Inline.ToMarkdownString();
                        //Console.WriteLine(spell.Name);
                    }
                }
                if (block is Markdig.Syntax.ParagraphBlock)
                {
                    if (block.IsNewItem())
                    {
                        return;
                    }
                    var paragraphBlock = block as Markdig.Syntax.ParagraphBlock;
                    this.Text += MarkdownExtensions.MarkdownToHtml(paragraphBlock.ToMarkdownString()) + "\n";
                }
                if (block is Markdig.Syntax.ListBlock)
                {
                    var listBlock = block as Markdig.Syntax.ListBlock;
                    //DumpListBlock(listBlock);
                    if (listBlock.BulletType == '-')
                    {
                        foreach (var inblock in listBlock)
                        {
                            //DumpBlock(inblock);
                            var regex = new Regex("(?<key>.*?): (?<value>.*)");
                            if (inblock is Markdig.Syntax.ListItemBlock)
                            {
                                var listItemBlock = inblock as Markdig.Syntax.ListItemBlock;
                                foreach (var ininblock in listItemBlock)
                                {
                                    //DumpBlock(ininblock);
                                    if (ininblock is Markdig.Syntax.ParagraphBlock)
                                    {
                                        var paragraphBlock = ininblock as Markdig.Syntax.ParagraphBlock;
                                        //DumpParagraphBlock(paragraphBlock);
                                        var str = paragraphBlock.Inline.ToMarkdownString();

                                        var properties = new List<Tuple<string, Action<Condition, string>>>()
                                        {
                                            new Tuple<string, Action<Condition, string>>("NameVO: ", (m, s) => m.NameVO = s),
                                        };

                                        foreach (var property in properties)
                                        {
                                            if (str.StartsWith(property.Item1))
                                            {
                                                property.Item2.Invoke(this, str.Substring(property.Item1.Length));
                                                break;
                                            }
                                        }

                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (var inblock in listBlock)
                        {
                            if (inblock is Markdig.Syntax.ListItemBlock)
                            {
                                var listItemBlock = inblock as Markdig.Syntax.ListItemBlock;
                                foreach (var ininblock in listItemBlock)
                                {
                                    //DumpBlock(ininblock);
                                    if (ininblock is Markdig.Syntax.ParagraphBlock)
                                    {
                                        var paragraphBlock = ininblock as Markdig.Syntax.ParagraphBlock;
                                        this.Text += listBlock.BulletType + " " + MarkdownExtensions.MarkdownToHtml(paragraphBlock.ToMarkdownString()) + "\n";
                                    }
                                }
                            }
                        }
                    }
                }
                else if (block is Markdig.Extensions.Tables.Table)
                {
                    var tableBlock = block as Markdig.Extensions.Tables.Table;
                    this.Text += "\n\n" + tableBlock.ToMarkdownString() + "\n\n";
                }

                enumerator.MoveNext();

            }
        }
    }
}
