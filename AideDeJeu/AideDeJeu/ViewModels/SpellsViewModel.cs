﻿using AideDeJeu.Services;
using AideDeJeu.Tools;
using AideDeJeuLib;
using AideDeJeuLib.Spells;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AideDeJeu.ViewModels
{
    public class SpellsViewModel : ItemsViewModel
    {
        public List<KeyValuePair<string, string>> Classes { get; set; } = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("", "Toutes" ),
            new KeyValuePair<string, string>("Barde", "Barde" ),
            new KeyValuePair<string, string>("Clerc", "Clerc" ),
            new KeyValuePair<string, string>("Druide", "Druide" ),
            new KeyValuePair<string, string>("Ensorceleur", "Ensorceleur" ),
            new KeyValuePair<string, string>("Magicien", "Magicien" ),
            new KeyValuePair<string, string>("Paladin", "Paladin" ),
            new KeyValuePair<string, string>("Rôdeur", "Rôdeur" ),
            new KeyValuePair<string, string>("Sorcier", "Sorcier" ),
        };

        public List<KeyValuePair<string, string>> Niveaux { get; set; } = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("0", "Sorts mineurs"),
            new KeyValuePair<string, string>("1", "Niveau 1"),
            new KeyValuePair<string, string>("2", "Niveau 2"),
            new KeyValuePair<string, string>("3", "Niveau 3"),
            new KeyValuePair<string, string>("4", "Niveau 4"),
            new KeyValuePair<string, string>("5", "Niveau 5"),
            new KeyValuePair<string, string>("6", "Niveau 6"),
            new KeyValuePair<string, string>("7", "Niveau 7"),
            new KeyValuePair<string, string>("8", "Niveau 8"),
            new KeyValuePair<string, string>("9", "Niveau 9"),
        };

        public List<KeyValuePair<string, string>> Ecoles { get; set; } = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("", "Toutes"),
            new KeyValuePair<string, string>("abjuration", "Abjuration"),
            new KeyValuePair<string, string>("divination", "Divination"),
            new KeyValuePair<string, string>("enchantement", "Enchantement"),
            new KeyValuePair<string, string>("vocation", "Évocation"),
            new KeyValuePair<string, string>("illusion", "Illusion"),
            new KeyValuePair<string, string>("invocation", "Invocation"),
            new KeyValuePair<string, string>("cromancie", "Nécromancie"),
            new KeyValuePair<string, string>("transmutation", "Transmutation"),
        };

        public List<KeyValuePair<string, string>> Rituels { get; set; } = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("", "Tous"),
            new KeyValuePair<string, string>("(rituel)", "Rituel"),
        };

        public List<KeyValuePair<string, string>> Sources { get; set; } = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("", "Toutes"),
            new KeyValuePair<string, string>("(SRD)", "SRD"),
            new KeyValuePair<string, string>("Player's Handbook", "PHB"),
            new KeyValuePair<string, string>("sup", "SCAG, XGtE"),
        };

        private int _Classe = 0;
        public int Classe
        {
            get
            {
                return _Classe;
            }
            set
            {
                SetProperty(ref _Classe, value);
                LoadItemsCommand.Execute(null);
            }
        }
        private int _NiveauMin = 0;
        public int NiveauMin
        {
            get
            {
                return _NiveauMin;
            }
            set
            {
                SetProperty(ref _NiveauMin, value);
                if (_NiveauMax < _NiveauMin)
                {
                    SetProperty(ref _NiveauMax, value, nameof(NiveauMax));
                }
                LoadItemsCommand.Execute(null);
            }
        }
        private int _NiveauMax = 9;
        public int NiveauMax
        {
            get
            {
                return _NiveauMax;
            }
            set
            {
                SetProperty(ref _NiveauMax, value);
                if (_NiveauMax < _NiveauMin)
                {
                    SetProperty(ref _NiveauMin, value, nameof(NiveauMin));
                }
                LoadItemsCommand.Execute(null);
            }
        }
        private int _Ecole = 0;
        public int Ecole
        {
            get
            {
                return _Ecole;
            }
            set
            {
                SetProperty(ref _Ecole, value);
                LoadItemsCommand.Execute(null);
            }
        }
        private int _Rituel = 0;
        public int Rituel
        {
            get
            {
                return _Rituel;
            }
            set
            {
                SetProperty(ref _Rituel, value);
                LoadItemsCommand.Execute(null);
            }
        }
        private int _Source = 1;
        public int Source
        {
            get
            {
                return _Source;
            }
            set
            {
                SetProperty(ref _Source, value);
                LoadItemsCommand.Execute(null);
            }
        }


        public SpellsViewModel(INavigator navigator, ObservableCollection<Item> items)
            : base(navigator, items)
        {
        }

        public override async Task ExecuteLoadItemsCommandAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                AllItems.Clear();
                IEnumerable<Spell> items = null;
                //using (var spellsScrappers = new SpellsScrappers())
                //{
                //    items = await spellsScrappers.GetSpells(classe: Classes[Classe].Key, niveauMin: Niveaux[NiveauMin].Key, niveauMax: Niveaux[NiveauMax].Key, ecole: Ecoles[Ecole].Key, rituel: Rituels[Rituel].Key, source: Sources[Source].Key);
                //}

                ItemDatabaseHelper<ItemDatabaseContext> helper = new ItemDatabaseHelper<ItemDatabaseContext>();
                items = await helper.GetSpellsAsync(classe: Classes[Classe].Key, niveauMin: Niveaux[NiveauMin].Key, niveauMax: Niveaux[NiveauMax].Key, ecole: Ecoles[Ecole].Key, rituel: Rituels[Rituel].Key, source: Sources[Source].Key);
                
                
                //try
                //{
                //ItemDatabaseHelper<ItemDatabaseContext> helper = new ItemDatabaseHelper<ItemDatabaseContext>();
                //await helper.AddOrUpdateSpellsAsync(items);
                //var items2 = await helper.GetSpellsAsync();
                //}
                //catch(Exception ex)
                //{
                //    Debug.WriteLine(ex);
                //}
                var aitems = items.ToArray();
                Array.Sort(aitems, new ItemComparer());
                foreach (var item in aitems)
                {
                    AllItems.Add(item);
                }
                FilterItems();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public override async Task ExecuteGotoItemCommandAsync(Item item)
        {
            await Navigator.GotoSpellDetailPageAsync(item as Spell);
        }

    }
}