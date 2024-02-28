// Decompiled with JetBrains decompiler
// Type: CGameManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50E6FD7C-AB91-4CD3-A1BF-6B78A5F552FF
// Assembly location: D:\Plague_Inc\PlagueIncEvolved_Data\Managed\Assembly-CSharp.dll

using AurochDigital.UGC;
using SimpleJSON;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

#nullable disable
public static class CGameManager
{
  public static IGame game;
  public static ISaves saves;
  public static IScores scores;
  public static DateTime currentGameDate;
  public static IGame.GameType gameType;
  public static IGame.GameType lastGameType;
  public static IPlayerInfo localPlayerInfo;
  public static bool pauseHud;
  public static bool pauseInterface;
  public static bool pausePopup;
  public static bool canPlaySFX;
  public static int multiplayerVersionRequired = 9999999;
  public static int betaMultiplayerVersionRequired = 9999999;
  public static int multiplayerLobbyRatingSearchStartBounds = 10;
  public static int multiplayerLobbyRatingSearchIncrement = 30;
  public static int multiplayerLobbyMinRefresh = 5;
  public static int multiplayerLobbyMaxRefresh = 10;
  public static int multiplayerLobbyDistanceWait1 = 120;
  public static int multiplayerLobbyDistanceWait2 = 240;
  public static int multiplayerIgnoreRankWaitTime = 240;
  public static int multiplayerIgnoreRatingOnVersion = 0;
  public static int multiplayerIgnoreLastPlayerWaitTime = 30;
  public static string dynamicNewsText;
  public static string dynamicPopupText;
  public static int lastSpeed;
  private static bool paused;
  private static float gameStartTime;
  private static Disease.EDiseaseType? gameTimeDiseaseType;
  public static bool usingDiseaseX = false;
  public const int UNSCHEDULED_FLIGHT_MINIMUM = 1000;
  public static int[] ScenarioRatingBands = new int[3]
  {
    0,
    35000,
    65000
  };
  public static int[] BoardGameRatingBands = new int[3]
  {
    0,
    50000000,
    100000000
  };
  public static int[] FakeNewsRatingBands = new int[3]
  {
    0,
    25000,
    55000
  };
  public static Disease.EDiseaseType[] ActiveDiseases = new Disease.EDiseaseType[11]
  {
    Disease.EDiseaseType.BACTERIA,
    Disease.EDiseaseType.VIRUS,
    Disease.EDiseaseType.FUNGUS,
    Disease.EDiseaseType.NEURAX,
    Disease.EDiseaseType.PARASITE,
    Disease.EDiseaseType.PRION,
    Disease.EDiseaseType.NECROA,
    Disease.EDiseaseType.NANO_VIRUS,
    Disease.EDiseaseType.BIO_WEAPON,
    Disease.EDiseaseType.SIMIAN_FLU,
    Disease.EDiseaseType.VAMPIRE
  };
  public static Disease.EDiseaseType[] ActiveDiseases_Cure = new Disease.EDiseaseType[8]
  {
    Disease.EDiseaseType.BACTERIA,
    Disease.EDiseaseType.VIRUS,
    Disease.EDiseaseType.PARASITE,
    Disease.EDiseaseType.FUNGUS,
    Disease.EDiseaseType.PRION,
    Disease.EDiseaseType.NANO_VIRUS,
    Disease.EDiseaseType.BIO_WEAPON,
    Disease.EDiseaseType.DISEASEX
  };
  public static Dictionary<Disease.EDiseaseType, string> EventsPaths = new Dictionary<Disease.EDiseaseType, string>()
  {
    {
      Disease.EDiseaseType.BACTERIA,
      "Events/standard_normal"
    },
    {
      Disease.EDiseaseType.NECROA,
      "Events/standard_necroa"
    },
    {
      Disease.EDiseaseType.SIMIAN_FLU,
      "Events/standard_simian"
    },
    {
      Disease.EDiseaseType.TUTORIAL,
      "Events/standard_tutorial"
    },
    {
      Disease.EDiseaseType.VAMPIRE,
      "Events/standard_vampire"
    },
    {
      Disease.EDiseaseType.CURE,
      "Events/standard_cure"
    },
    {
      Disease.EDiseaseType.CURETUTORIAL,
      "Events/standard_cure"
    }
  };
  public static List<Disease.EDiseaseType> DiseaseUnlockOrder = new List<Disease.EDiseaseType>()
  {
    Disease.EDiseaseType.BACTERIA,
    Disease.EDiseaseType.VIRUS,
    Disease.EDiseaseType.FUNGUS,
    Disease.EDiseaseType.NEURAX,
    Disease.EDiseaseType.PARASITE,
    Disease.EDiseaseType.PRION,
    Disease.EDiseaseType.NECROA,
    Disease.EDiseaseType.NANO_VIRUS,
    Disease.EDiseaseType.BIO_WEAPON,
    Disease.EDiseaseType.SIMIAN_FLU,
    Disease.EDiseaseType.VAMPIRE
  };
  public static List<Disease.ECureScenario> CureUnlockOrder = new List<Disease.ECureScenario>()
  {
    Disease.ECureScenario.Cure_Standard,
    Disease.ECureScenario.Cure_Virus,
    Disease.ECureScenario.Cure_Parasite,
    Disease.ECureScenario.Cure_Fungus,
    Disease.ECureScenario.Cure_Prion,
    Disease.ECureScenario.Cure_Nanovirus,
    Disease.ECureScenario.Cure_Bioweapon,
    Disease.ECureScenario.Cure_DiseaseX
  };
  private static Dictionary<Disease.EDiseaseType, string> DiseaseNames = new Dictionary<Disease.EDiseaseType, string>()
  {
    {
      Disease.EDiseaseType.BACTERIA,
      "Bacteria"
    },
    {
      Disease.EDiseaseType.PRION,
      "Prion"
    },
    {
      Disease.EDiseaseType.VIRUS,
      "Virus"
    },
    {
      Disease.EDiseaseType.PARASITE,
      "Parasite"
    },
    {
      Disease.EDiseaseType.FUNGUS,
      "Fungus"
    },
    {
      Disease.EDiseaseType.NANO_VIRUS,
      "Nano-Virus"
    },
    {
      Disease.EDiseaseType.BIO_WEAPON,
      "Bio-Weapon"
    },
    {
      Disease.EDiseaseType.NEURAX,
      "Neurax Worm"
    },
    {
      Disease.EDiseaseType.NECROA,
      "Necroa Virus"
    },
    {
      Disease.EDiseaseType.SIMIAN_FLU,
      "Simian Flu"
    },
    {
      Disease.EDiseaseType.TUTORIAL,
      "Tutorial"
    },
    {
      Disease.EDiseaseType.VAMPIRE,
      "Shadow Plague"
    },
    {
      Disease.EDiseaseType.FAKE_NEWS,
      "Fake News"
    },
    {
      Disease.EDiseaseType.DISEASEX,
      "Disease X"
    }
  };
  public static Dictionary<Disease.EDiseaseType, string> DiseaseDescriptions = new Dictionary<Disease.EDiseaseType, string>()
  {
    {
      Disease.EDiseaseType.BACTERIA,
      "Most common cause of Plague. Unlimited potential"
    },
    {
      Disease.EDiseaseType.PRION,
      "Slow, subtle and extremely complex pathogen hidden inside the brain"
    },
    {
      Disease.EDiseaseType.VIRUS,
      "A rapidly mutating pathogen which is extremely hard to control"
    },
    {
      Disease.EDiseaseType.PARASITE,
      "Parasitic lifestyle prevents DNA alteration from every day infection"
    },
    {
      Disease.EDiseaseType.FUNGUS,
      "Fungal spores struggle to travel long distances without special effort"
    },
    {
      Disease.EDiseaseType.NANO_VIRUS,
      "Out of control, microscopic machine with a built in kill switch"
    },
    {
      Disease.EDiseaseType.BIO_WEAPON,
      "Exceptionally lethal pathogen that kills everything it touches"
    },
    {
      Disease.EDiseaseType.NEURAX,
      "Manipulative organism that burrows into the brain"
    },
    {
      Disease.EDiseaseType.NECROA,
      "Unclassified. Early analysis suggests extreme regenerative abilities."
    },
    {
      Disease.EDiseaseType.SIMIAN_FLU,
      "Genetically modified. Increases ape intelligence but untested on humans..."
    },
    {
      Disease.EDiseaseType.TUTORIAL,
      "Tutorial Description"
    },
    {
      Disease.EDiseaseType.VAMPIRE,
      "Sentient, mutagenic pathogen. Triggers a powerful thirst for blood!"
    }
  };
  public static Dictionary<Disease.EDiseaseType, string> CureDiseaseDescriptions = new Dictionary<Disease.EDiseaseType, string>()
  {
    {
      Disease.EDiseaseType.BACTERIA,
      "Most common disease. Well understood and easy to detect"
    },
    {
      Disease.EDiseaseType.PRION,
      "Slow, subtle and extremely complex pathogen hidden inside the brain"
    },
    {
      Disease.EDiseaseType.VIRUS,
      "Underestimated, infectious, and difficult to trace"
    },
    {
      Disease.EDiseaseType.PARASITE,
      "A complex parasitic organism hiding in plain sight"
    },
    {
      Disease.EDiseaseType.FUNGUS,
      "Fungal spores cause unpredictable spread patterns. Difficult to contain"
    },
    {
      Disease.EDiseaseType.NANO_VIRUS,
      "Out of control, microscopic machine with a built in kill switch"
    },
    {
      Disease.EDiseaseType.BIO_WEAPON,
      "Exceptionally lethal pathogen that kills everything it touches"
    },
    {
      Disease.EDiseaseType.NEURAX,
      ""
    },
    {
      Disease.EDiseaseType.NECROA,
      ""
    },
    {
      Disease.EDiseaseType.SIMIAN_FLU,
      ""
    },
    {
      Disease.EDiseaseType.TUTORIAL,
      ""
    },
    {
      Disease.EDiseaseType.VAMPIRE,
      ""
    },
    {
      Disease.EDiseaseType.DISEASEX,
      "Disease X: The Next Pandemic"
    }
  };
  public static Dictionary<Disease.EDiseaseType, string> DiseaseDescriptionsMP = new Dictionary<Disease.EDiseaseType, string>()
  {
    {
      Disease.EDiseaseType.BACTERIA,
      "MP_Bacteria_Description"
    },
    {
      Disease.EDiseaseType.PRION,
      "Slow, subtle and extremely complex pathogen hidden inside the brain"
    },
    {
      Disease.EDiseaseType.VIRUS,
      "MP_Virus_Description"
    },
    {
      Disease.EDiseaseType.PARASITE,
      "MP_Parasite_Description"
    },
    {
      Disease.EDiseaseType.FUNGUS,
      "MP_Fungus_Description"
    },
    {
      Disease.EDiseaseType.NANO_VIRUS,
      "Out of control, microscopic machine with a built in kill switch"
    },
    {
      Disease.EDiseaseType.BIO_WEAPON,
      "MP_Bioweapon_Description"
    },
    {
      Disease.EDiseaseType.NEURAX,
      "Manipulative organism that burrows into the brain"
    },
    {
      Disease.EDiseaseType.NECROA,
      "Unclassified. Early analysis suggests extreme regenerative abilities."
    },
    {
      Disease.EDiseaseType.SIMIAN_FLU,
      "Genetically modified. Increases ape intelligence but untested on humans..."
    },
    {
      Disease.EDiseaseType.VAMPIRE,
      "Sentient, mutagenic pathogen. Triggers a powerful thirst for blood!"
    }
  };
  public static Dictionary<Disease.EDiseaseType, string> DiseaseDescriptionLocked = new Dictionary<Disease.EDiseaseType, string>()
  {
    {
      Disease.EDiseaseType.PRION,
      "FE_Disease_Creation_Locked_Message_Prion"
    },
    {
      Disease.EDiseaseType.VIRUS,
      "FE_Disease_Creation_Locked_Message_Virus"
    },
    {
      Disease.EDiseaseType.PARASITE,
      "FE_Disease_Creation_Locked_Message_Parasite"
    },
    {
      Disease.EDiseaseType.FUNGUS,
      "FE_Disease_Creation_Locked_Message_Fungus"
    },
    {
      Disease.EDiseaseType.NANO_VIRUS,
      "FE_Disease_Creation_Locked_Message_Nano_Virus"
    },
    {
      Disease.EDiseaseType.BIO_WEAPON,
      "FE_Disease_Creation_Locked_Message_Bio_Weapon"
    },
    {
      Disease.EDiseaseType.NEURAX,
      "FE_Disease_Creation_Locked_Message_Neurax"
    },
    {
      Disease.EDiseaseType.NECROA,
      "FE_Disease_Creation_Locked_Message_Necroa"
    },
    {
      Disease.EDiseaseType.SIMIAN_FLU,
      "FE_Disease_Creation_Locked_Message_Simian_Flu"
    },
    {
      Disease.EDiseaseType.VAMPIRE,
      "FE_Disease_Creation_Locked_Message_Vampire"
    },
    {
      Disease.EDiseaseType.DISEASEX,
      "FE_Disease_Creation_Locked_Message_DiseaseX"
    }
  };
  public static Dictionary<Disease.EDiseaseType, string> CureDiseaseDescriptionLocked = new Dictionary<Disease.EDiseaseType, string>()
  {
    {
      Disease.EDiseaseType.PRION,
      "FE_CureDisease_Creation_Locked_Message_Parasite"
    },
    {
      Disease.EDiseaseType.VIRUS,
      "FE_CureDisease_Creation_Locked_Message_Virus"
    },
    {
      Disease.EDiseaseType.PARASITE,
      "FE_CureDisease_Creation_Locked_Message_Fungus"
    },
    {
      Disease.EDiseaseType.FUNGUS,
      "FE_CureDisease_Creation_Locked_Message_Prion"
    },
    {
      Disease.EDiseaseType.NANO_VIRUS,
      "FE_CureDisease_Creation_Locked_Message_Nano_Virus"
    },
    {
      Disease.EDiseaseType.BIO_WEAPON,
      "FE_CureDisease_Creation_Locked_Message_Bio_Weapon"
    },
    {
      Disease.EDiseaseType.DISEASEX,
      "FE_CureDisease_Creation_Locked_Message_DiseaseX"
    }
  };
  public static Dictionary<Disease.EDiseaseType, EAchievement?> DiseaseCompletionAchievements = new Dictionary<Disease.EDiseaseType, EAchievement?>()
  {
    {
      Disease.EDiseaseType.BACTERIA,
      new EAchievement?(EAchievement.A_CompleteBacteria)
    },
    {
      Disease.EDiseaseType.VIRUS,
      new EAchievement?(EAchievement.A_CompleteVirus)
    },
    {
      Disease.EDiseaseType.FUNGUS,
      new EAchievement?(EAchievement.A_CompleteFungus)
    },
    {
      Disease.EDiseaseType.NEURAX,
      new EAchievement?(EAchievement.A_CompleteNeurax)
    },
    {
      Disease.EDiseaseType.PARASITE,
      new EAchievement?(EAchievement.A_CompleteParasite)
    },
    {
      Disease.EDiseaseType.PRION,
      new EAchievement?(EAchievement.A_CompletePrion)
    },
    {
      Disease.EDiseaseType.NANO_VIRUS,
      new EAchievement?(EAchievement.A_CompleteNanoVirus)
    },
    {
      Disease.EDiseaseType.BIO_WEAPON,
      new EAchievement?(EAchievement.A_CompleteBioWeapon)
    },
    {
      Disease.EDiseaseType.NECROA,
      new EAchievement?(EAchievement.A_CompleteNecroa)
    },
    {
      Disease.EDiseaseType.SIMIAN_FLU,
      new EAchievement?()
    },
    {
      Disease.EDiseaseType.VAMPIRE,
      new EAchievement?()
    }
  };
  public static Dictionary<Disease.EDiseaseType, EAchievement?> DiseaseCompletionMegaAchievements = new Dictionary<Disease.EDiseaseType, EAchievement?>()
  {
    {
      Disease.EDiseaseType.BACTERIA,
      new EAchievement?(EAchievement.A_BacteriaMaster)
    },
    {
      Disease.EDiseaseType.VIRUS,
      new EAchievement?(EAchievement.A_VirusMaster)
    },
    {
      Disease.EDiseaseType.FUNGUS,
      new EAchievement?(EAchievement.A_FungusMaster)
    },
    {
      Disease.EDiseaseType.NEURAX,
      new EAchievement?(EAchievement.A_NeuraxMaster)
    },
    {
      Disease.EDiseaseType.PARASITE,
      new EAchievement?(EAchievement.A_ParasiteMaster)
    },
    {
      Disease.EDiseaseType.PRION,
      new EAchievement?(EAchievement.A_PrionMaster)
    },
    {
      Disease.EDiseaseType.NANO_VIRUS,
      new EAchievement?(EAchievement.A_NanovirusMaster)
    },
    {
      Disease.EDiseaseType.BIO_WEAPON,
      new EAchievement?(EAchievement.A_BioweaponMaster)
    },
    {
      Disease.EDiseaseType.NECROA,
      new EAchievement?(EAchievement.A_NecroaMaster)
    },
    {
      Disease.EDiseaseType.SIMIAN_FLU,
      new EAchievement?(EAchievement.A_simianmaster)
    },
    {
      Disease.EDiseaseType.VAMPIRE,
      new EAchievement?(EAchievement.A_vampiremaster)
    }
  };
  public static Dictionary<Disease.ECureScenario, EAchievement?> CureCompletionAchievements = new Dictionary<Disease.ECureScenario, EAchievement?>()
  {
    {
      Disease.ECureScenario.Cure_Standard,
      new EAchievement?(EAchievement.A_bacteriacured)
    },
    {
      Disease.ECureScenario.Cure_Virus,
      new EAchievement?(EAchievement.A_viruscured)
    },
    {
      Disease.ECureScenario.Cure_Parasite,
      new EAchievement?(EAchievement.A_parasitecured)
    },
    {
      Disease.ECureScenario.Cure_Fungus,
      new EAchievement?(EAchievement.A_funguscured)
    },
    {
      Disease.ECureScenario.Cure_Prion,
      new EAchievement?(EAchievement.A_prioncured)
    },
    {
      Disease.ECureScenario.Cure_Nanovirus,
      new EAchievement?(EAchievement.A_nanoviruscured)
    },
    {
      Disease.ECureScenario.Cure_Bioweapon,
      new EAchievement?(EAchievement.A_bioweaponcured)
    }
  };
  public static Dictionary<Disease.ECureScenario, EAchievement?> CureCompletionMegaAchievements = new Dictionary<Disease.ECureScenario, EAchievement?>();
  public static Dictionary<string, EAchievement> ScenarioCompletionAchievements = new Dictionary<string, EAchievement>()
  {
    {
      "black_death",
      EAchievement.A_completeblackdeath
    },
    {
      "pirate_plague",
      EAchievement.A_completepirateplague
    },
    {
      "xenophobia",
      EAchievement.A_completexenophobia
    },
    {
      "volcanic_ash",
      EAchievement.A_completevolcanicash
    },
    {
      "global_warming",
      EAchievement.A_completeglobalwarming
    },
    {
      "ice_age",
      EAchievement.A_completeiceage
    },
    {
      "sovereign_default",
      EAchievement.A_completesovereigndefault
    },
    {
      "mirror_earth",
      EAchievement.A_completemirrorearth
    },
    {
      "swine_flu",
      EAchievement.A_completeswineflu
    },
    {
      "smallpox",
      EAchievement.A_completesmallpox
    },
    {
      "golden_age",
      EAchievement.A_completegoldenage
    },
    {
      "shut_down_everything",
      EAchievement.A_completeshutdowneverything
    },
    {
      "who_cares",
      EAchievement.A_completewhocares
    },
    {
      "created_equal",
      EAchievement.A_completecreatedequal
    },
    {
      "artificial_organs",
      EAchievement.A_ArtificialOrgans
    },
    {
      "frozen_virus",
      EAchievement.A_FrozenVirus
    },
    {
      "nipah_virus",
      EAchievement.A_NipahVirus
    },
    {
      "unknown_origin",
      EAchievement.A_UnknownOrigin
    },
    {
      "teleportation",
      EAchievement.A_completeteleportation
    },
    {
      "christmas_spirit",
      EAchievement.A_completesantaslittlehelper
    },
    {
      "mad_cow_disease",
      EAchievement.A_completemaddcow
    },
    {
      "flight_club",
      EAchievement.A_completeflightclub
    },
    {
      "where_is_everyone",
      EAchievement.A_completewhereiseveryone
    },
    {
      "board_game",
      EAchievement.A_completeultimateboardgames
    },
    {
      "science_denial",
      EAchievement.A_completesciencedenial
    }
  };
  public static Dictionary<string, EAchievement> ScenarioCompletionMegaAchievements = new Dictionary<string, EAchievement>()
  {
    {
      "pirate_plague",
      EAchievement.A_supersparrow
    },
    {
      "volcanic_ash",
      EAchievement.A_lavagod
    },
    {
      "shut_down_everything",
      EAchievement.A_mrpresident
    }
  };
  public static Dictionary<Disease.ECheatType, string> CheatDescriptions = new Dictionary<Disease.ECheatType, string>()
  {
    {
      Disease.ECheatType.HIDDEN,
      "No Governments cheat: Humans will never take action against you"
    },
    {
      Disease.ECheatType.UNLIMITED,
      "Unlimited DNA cheat: Evolve with the power of a god"
    },
    {
      Disease.ECheatType.IMMUNE,
      "No Cure Cheat: Impossible to find a cure for the Plague"
    }
  };
  public static List<string> Difficulties = new List<string>()
  {
    "casual",
    "normal",
    "brutal",
    "mega-brutal"
  };
  public static ICollection<string> DefaultScenarios = (ICollection<string>) new HashSet<string>()
  {
    "black_death",
    "christmas_spirit",
    "mad_cow_disease",
    "science_denial",
    "board_game",
    "fake_news"
  };
  public static Dictionary<uint, string> DifficultyNames = new Dictionary<uint, string>()
  {
    {
      0U,
      "FE_Difficulty_Text_Easy_Title"
    },
    {
      1U,
      "FE_Difficulty_Text_Medium_Title"
    },
    {
      2U,
      "FE_Difficulty_Text_Hard_Title"
    },
    {
      3U,
      "FE_Mega_Brutal"
    }
  };
  public static Dictionary<uint, string> DifficultyDescriptions = new Dictionary<uint, string>()
  {
    {
      0U,
      "FE_Difficulty_Text_Easy_Text"
    },
    {
      1U,
      "FE_Difficulty_Text_Medium_Text"
    },
    {
      2U,
      "FE_Difficulty_Text_Hard_Text"
    },
    {
      3U,
      "FE_Difficulty_Text_Mega_Brutal_Text"
    }
  };
  public static Dictionary<uint, string> DifficultyIcons = new Dictionary<uint, string>()
  {
    {
      0U,
      "Rating_Copper"
    },
    {
      1U,
      "Rating_Silver"
    },
    {
      2U,
      "Rating_Gold"
    },
    {
      3U,
      "Rating_Mega"
    }
  };
  public static string[] CureDifficultyLabel = new string[4]
  {
    "For new players or those\nwanting a quick game",
    "For experienced pandemic strategists",
    "For strategic geniuses living in concrete bunkers",
    "For players who thought Brutal was Casual"
  };
  public static string[][] CureDifficultyBullets = new string[4][]
  {
    new string[3]
    {
      "Full international co-operation",
      "People love wearing masks",
      "Clapping scares disease away"
    },
    new string[3]
    {
      "Politicians vaguely competent",
      "Experts mostly listened to",
      "Healthcare systems unprepared"
    },
    new string[3]
    {
      "Leaders ignore science",
      "Sick people inject disinfectant",
      "Doctors arrested for reporting disease"
    },
    new string[3]
    {
      "Everyone believes fake news",
      "Sick people given hugs",
      "Doctors play Plague Inc. all day"
    }
  };
  public static Dictionary<Gene.EGeneCategory, string> GeneCategoryNames = new Dictionary<Gene.EGeneCategory, string>()
  {
    {
      Gene.EGeneCategory.dna,
      "DNA Genes"
    },
    {
      Gene.EGeneCategory.environmental,
      "Environment Genes"
    },
    {
      Gene.EGeneCategory.evolve,
      "Evolution Genes"
    },
    {
      Gene.EGeneCategory.mutation,
      "Mutation Genes"
    },
    {
      Gene.EGeneCategory.travel,
      "Travel Genes"
    },
    {
      Gene.EGeneCategory.zombie,
      "Necroa Virus Genes"
    },
    {
      Gene.EGeneCategory.simian1,
      "Artificial Genes"
    },
    {
      Gene.EGeneCategory.simian2,
      "Ape Genes"
    },
    {
      Gene.EGeneCategory.blood,
      "Blood Genes"
    },
    {
      Gene.EGeneCategory.flight,
      "Flight Genes"
    },
    {
      Gene.EGeneCategory.shadow,
      "Shadow Genes"
    },
    {
      Gene.EGeneCategory.cure_abilities,
      "Abilities"
    },
    {
      Gene.EGeneCategory.cure_country,
      "IG_Cure_Country"
    },
    {
      Gene.EGeneCategory.cure_operation,
      "IG_Cure_Operation"
    },
    {
      Gene.EGeneCategory.cure_quarantine,
      "IG_Cure_Quarantine"
    },
    {
      Gene.EGeneCategory.cure_transmission,
      "IG_Cure_Transmission"
    }
  };
  public static Dictionary<Disease.EDiseaseType, int[]> SpeedrunScoreThresholds = new Dictionary<Disease.EDiseaseType, int[]>()
  {
    {
      Disease.EDiseaseType.BACTERIA,
      new int[5]{ 865, 665, 515, 415, 365 }
    },
    {
      Disease.EDiseaseType.BIO_WEAPON,
      new int[5]{ 700, 500, 350, 250, 200 }
    },
    {
      Disease.EDiseaseType.FUNGUS,
      new int[5]{ 700, 500, 350, 250, 200 }
    },
    {
      Disease.EDiseaseType.NANO_VIRUS,
      new int[5]{ 750, 550, 400, 300, 250 }
    },
    {
      Disease.EDiseaseType.NECROA,
      new int[5]{ 800, 600, 450, 350, 300 }
    },
    {
      Disease.EDiseaseType.SIMIAN_FLU,
      new int[5]{ 800, 600, 450, 350, 300 }
    },
    {
      Disease.EDiseaseType.NEURAX,
      new int[5]{ 700, 500, 350, 250, 200 }
    },
    {
      Disease.EDiseaseType.PARASITE,
      new int[5]{ 850, 650, 500, 400, 350 }
    },
    {
      Disease.EDiseaseType.PRION,
      new int[5]{ 850, 650, 500, 400, 350 }
    },
    {
      Disease.EDiseaseType.VIRUS,
      new int[5]{ 760, 560, 410, 310, 260 }
    }
  };
  public static List<int> DefconThresholds = new List<int>()
  {
    5,
    10,
    18,
    30,
    40,
    60
  };
  public static Dictionary<IGame.GameType, string> GameTypeNames = new Dictionary<IGame.GameType, string>()
  {
    {
      IGame.GameType.Cure,
      "FE_Game_Mode_Cure"
    },
    {
      IGame.GameType.Classic,
      "FE_Game_Mode_Classic"
    },
    {
      IGame.GameType.Custom,
      "FE_Game_Mode_Custom_Scenarios"
    },
    {
      IGame.GameType.Invalid,
      "FE_Game_Mode_Classic"
    },
    {
      IGame.GameType.Official,
      "FE_Game_Mode_Official_Scenarios"
    },
    {
      IGame.GameType.SpeedRun,
      "Speed Run"
    },
    {
      IGame.GameType.Tutorial,
      "FE_Game_Mode_Tutorial"
    },
    {
      IGame.GameType.VersusMP,
      "MP_Versus"
    },
    {
      IGame.GameType.CoopMP,
      "MP_Coop"
    }
  };
  private static IDictionary<int, CGameManager.AchievementData> achievementLookup;
  private static CGameManager.AchievementData[] achievementData = new CGameManager.AchievementData[236]
  {
    new CGameManager.AchievementData(EAchievement.A_AssumingDirectControlPC, "Assuming Direct Control", "Win a game by enslaving humanity with the Neurax Worm"),
    new CGameManager.AchievementData(EAchievement.A_WormFood, "Worm Food", "Win a game by eradicating humanity with the Neurax Worm"),
    new CGameManager.AchievementData(EAchievement.A_WormFoodTest, "", "", false),
    new CGameManager.AchievementData(EAchievement.A_NotAnotherZombieGame, "Not Another Zombie Game", "Win a game with the Necroa Virus without making a single zombie"),
    new CGameManager.AchievementData(EAchievement.A_TheGloriousDead, "The Glorious Dead", "Win a game with the Necroa Virus"),
    new CGameManager.AchievementData(EAchievement.A_InsaneBolt, "Insane Bolt", "Sprint to victory and set a world record by winning the game with Bacteria in under 365 days"),
    new CGameManager.AchievementData(EAchievement.A_CompleteBacteria, "STEAM_Bacteria_Victory_Title", "STEAM_Bacteria_Victory_Text"),
    new CGameManager.AchievementData(EAchievement.A_CompleteVirus, "STEAM_Virus_Victory_Title", "STEAM_Virus_Victory_Text"),
    new CGameManager.AchievementData(EAchievement.A_CompleteFungus, "STEAM_Fungus_Victory_Title", "STEAM_Fungus_Victory_Text"),
    new CGameManager.AchievementData(EAchievement.A_CompleteNeurax, "STEAM_Neurax_Title", "STEAM_Neurax_Text"),
    new CGameManager.AchievementData(EAchievement.A_CompleteParasite, "STEAM_Parasite_Victory_Title", "STEAM_Parasite_Victory_Text"),
    new CGameManager.AchievementData(EAchievement.A_CompletePrion, "STEAM_Prion_Victory_Title", "STEAM_Prion_Victory_Text"),
    new CGameManager.AchievementData(EAchievement.A_CompleteNecroa, "", "", false),
    new CGameManager.AchievementData(EAchievement.A_CompleteNanoVirus, "STEAM_Nano_Virus_Victory_Title", "STEAM_NanoVirus_Victory_Text"),
    new CGameManager.AchievementData(EAchievement.A_CompleteBioWeapon, "STEAM_Bio_Weapon_Title", "STEAM_Bio_Weapon_Text"),
    new CGameManager.AchievementData(EAchievement.A_UnlockCheats, "STEAM_Cheat_Victory_Title", "STEAM_Cheat_Victory_Text"),
    new CGameManager.AchievementData(EAchievement.A_InvoluntaryWithdrawal, "Z Com: Enemy Undead", "Destroy a Z Com fortress"),
    new CGameManager.AchievementData(EAchievement.A_IsItABird, "Is it a bird?", "Discover the Vampire Bat combo"),
    new CGameManager.AchievementData(EAchievement.A_RedRain, "Red Rain", "Discover the Profuse Bleeding combo"),
    new CGameManager.AchievementData(EAchievement.A_ContaminatedPackage, "Contaminated Package", "Let humans spread the disease via blood transfusion"),
    new CGameManager.AchievementData(EAchievement.A_OlympicSpoiler, "Olympic Spoiler", "Help the Olympics go viral in the UK"),
    new CGameManager.AchievementData(EAchievement.A_RussianNuclearRetaliation, "Russian Nuclear Retaliation", "Make the USA nuke Russia"),
    new CGameManager.AchievementData(EAchievement.A_ChineseNuclearRetaliation, "Chinese Nuclear Retaliation", "Make the USA nuke China"),
    new CGameManager.AchievementData(EAchievement.A_LongShot, "Long Shot", "Discover the Projectile Vomiting combo"),
    new CGameManager.AchievementData(EAchievement.A_BrownStreets, "Brown Streets", "Discover the Public Defecation combo"),
    new CGameManager.AchievementData(EAchievement.A_Brainzzzz, "Brainzzzz", "Discover the  Waking Dead combo"),
    new CGameManager.AchievementData(EAchievement.A_PlagueInSpace, "Plague in Space", "Infect astronauts before they launch a space mission"),
    new CGameManager.AchievementData(EAchievement.A_EvolveYourDisease, "Evolve your disease", "Evolve your disease to become stronger"),
    new CGameManager.AchievementData(EAchievement.A_OinkOink, "Oink oink", "Discover the Swineflu Combo"),
    new CGameManager.AchievementData(EAchievement.A_UhOh, "Uh Oh", "Discover the Oops symptom combo"),
    new CGameManager.AchievementData(EAchievement.A_PeerPressure, "Peer Pressure", "Have your plague discovered after riots force a government investigation"),
    new CGameManager.AchievementData(EAchievement.A_StalkersDelight, "STALKERs delight", "Cause a nuclear meltdown"),
    new CGameManager.AchievementData(EAchievement.A_UseYourHead, "Use your head", "Discover the Cranial Dispersion combo"),
    new CGameManager.AchievementData(EAchievement.A_Jaws, "Jaws", "Discover the Blood in the Air combo"),
    new CGameManager.AchievementData(EAchievement.A_Tasty, "Tasty", "Discover the Bath Time combo"),
    new CGameManager.AchievementData(EAchievement.A_Tank, "Tank", "Discover the Tank combo"),
    new CGameManager.AchievementData(EAchievement.A_Boomer, "Boomer", "Discover the Boomer combo"),
    new CGameManager.AchievementData(EAchievement.A_Runner, "Runner", "Discover the Runner combo"),
    new CGameManager.AchievementData(EAchievement.A_Spitter, "Spitter", "Discover the Spitter combo"),
    new CGameManager.AchievementData(EAchievement.A_WalkingContradiction, "Walking Contradiction", "Discover the Walking Contradiction combo"),
    new CGameManager.AchievementData(EAchievement.A_WhoNeedsDEET, "Who needs DEET", "Avoid insect bites"),
    new CGameManager.AchievementData(EAchievement.A_NotStarter, "Non Starter", "Stop DarkWater from analysing the Necroa Virus"),
    new CGameManager.AchievementData(EAchievement.A_DeadEnd, "Dead End", "Stop DarkWater from discovering a Necroa Virus weakness"),
    new CGameManager.AchievementData(EAchievement.A_BigBang, "Big Bang", "Kill the DarkWater research team"),
    new CGameManager.AchievementData(EAchievement.A_GettingColder, "Getting Colder", "Trick the Egyptian DNA tests"),
    new CGameManager.AchievementData(EAchievement.A_ItsATrap, "It's a Trap!", "Wipe out the Giza expedition"),
    new CGameManager.AchievementData(EAchievement.A_RevengeOfOsiris, "Revenge of Osiris", "Invalidate the knowledge of the pharaohs"),
    new CGameManager.AchievementData(EAchievement.A_HideAndSeek, "Hide and Seek", "Prevent link with Chernobyl exclusion zone"),
    new CGameManager.AchievementData(EAchievement.A_TestThis, "Test This!", "Force Chernobyl research to be put on hold"),
    new CGameManager.AchievementData(EAchievement.A_BreatheDeep, "Breathe Deep", "Infect the Chernobyl research team"),
    new CGameManager.AchievementData(EAchievement.A_DueDiligence, "Due Diligence", "Stop PfiGlax finding a link to the Necroa Virus"),
    new CGameManager.AchievementData(EAchievement.A_BananaSkin, "Banana Skin", "Prevent PfiGlax modifying the Necroa Virus"),
    new CGameManager.AchievementData(EAchievement.A_DontAskMe, "Don't Ask Me", "Make the PfiGlax modification project fail"),
    new CGameManager.AchievementData(EAchievement.A_BottleSmasher, "Bottle Smasher", "Smash a Blue cure bubble and slow down the Research Team"),
    new CGameManager.AchievementData(EAchievement.A_TrojanHorse, "Trojan Horse", "Create a trojan plane and use it to infect a new country with the Neurax Worm"),
    new CGameManager.AchievementData(EAchievement.A_TouchscreenTrash, "Touchscreen Trash", "Disrupt the iCure and stop it helping cure research"),
    new CGameManager.AchievementData(EAchievement.A_TheEndPlague, "The End Plague", "Win one game with any disease type and any difficulty"),
    new CGameManager.AchievementData(EAchievement.A_RMSWatchList, "RMS Watch List", "Get your Plague on the RMS Watch List"),
    new CGameManager.AchievementData(EAchievement.A_PatientWho, "Patient Who?", "Stop the CDC finding Patient Zero"),
    new CGameManager.AchievementData(EAchievement.A_FlashMob, "Flash Mob", "Successfully use the Zombie Horde active ability"),
    new CGameManager.AchievementData(EAchievement.A_IllBeBack, "I'll Be Back", "Successfully use the Zombie re-animate active ability"),
    new CGameManager.AchievementData(EAchievement.A_completeblackdeath, "Complete Black Death", "Score 3 biohazards in the Black Death scenario"),
    new CGameManager.AchievementData(EAchievement.A_completepirateplague, "Complete Pirate Plague", "Score 3 biohazards in the Pirate Plague scenario"),
    new CGameManager.AchievementData(EAchievement.A_completexenophobia, "Complete Xenophobia", "Score 3 biohazards in the Xenophobia scenario"),
    new CGameManager.AchievementData(EAchievement.A_completevolcanicash, "Complete Volcanic Ash", "Score 3 biohazards in the Volcanic Ash scenario"),
    new CGameManager.AchievementData(EAchievement.A_completeglobalwarming, "Complete Global Warming", "Score 3 biohazards in the Global Warming scenario"),
    new CGameManager.AchievementData(EAchievement.A_completeiceage, "Complete Ice Age", "Score 3 biohazards in the Ice Age scenario"),
    new CGameManager.AchievementData(EAchievement.A_completesovereigndefault, "Complete Sovereign Default", "Score 3 biohazards in the Sovereign Default scenario"),
    new CGameManager.AchievementData(EAchievement.A_completemirrorearth, "Complete Mirror Earth", "Score 3 biohazards in the Mirror Earth scenario"),
    new CGameManager.AchievementData(EAchievement.A_completeswineflu, "Complete Swine Flu", "Score 3 biohazards in the Swine Flu scenario"),
    new CGameManager.AchievementData(EAchievement.A_completesmallpox, "Complete Smallpox", "Score 3 biohazards in the Smallpox scenario"),
    new CGameManager.AchievementData(EAchievement.A_completegoldenage, "Complete Golden Age", "Score 3 biohazards in the Golden Age scenario"),
    new CGameManager.AchievementData(EAchievement.A_completeshutdowneverything, "Complete Shut Down Everything", "Score 3 biohazards in the Shut Down Everything scenario"),
    new CGameManager.AchievementData(EAchievement.A_completewhocares, "Complete Who Cares", "Score 3 biohazards in the Who Cares scenario"),
    new CGameManager.AchievementData(EAchievement.A_completecreatedequal, "Complete Created Equal", "Score 3 biohazards in the Created Equal scenario"),
    new CGameManager.AchievementData(EAchievement.A_supersparrow, "STEAM_Super_Sparrow_Title", "STEAM_Super_Sparrow_Text"),
    new CGameManager.AchievementData(EAchievement.A_lavagod, "STEAM_Lava_God_Title", "STEAM_Lava_God_Text"),
    new CGameManager.AchievementData(EAchievement.A_mrpresident, "STEAM_Mr_President_Title", "STEAM_Mr_President_Text"),
    new CGameManager.AchievementData(EAchievement.A_UnknownOrigin, "Unknown Origin", "Score 3 biohazards in the Unknown Origin scenario"),
    new CGameManager.AchievementData(EAchievement.A_NipahVirus, "STEAM_Nipah_Virus_Title", "STEAM_Nipah_Virus_Text"),
    new CGameManager.AchievementData(EAchievement.A_FrozenVirus, "STEAM_Frozen_Virus_Title", "STEAM_Frozen_Virus_Text"),
    new CGameManager.AchievementData(EAchievement.A_ArtificialOrgans, "STEAM_Artificial_Organs_Title", "STEAM_Artificial_Organs_Text"),
    new CGameManager.AchievementData(EAchievement.A_CallPETA, "Call PETA", "Make a celebrity cry"),
    new CGameManager.AchievementData(EAchievement.A_ContagionCancelled, "Contagion Cancelled", "Prevent a film about your plague being made"),
    new CGameManager.AchievementData(EAchievement.A_WhoNeedsScience, "Who needs Science", "Show the world how useful homeopathy is"),
    new CGameManager.AchievementData(EAchievement.A_WhoNeedsBrains, "Who needs brains", "Take humanity back to the stone age"),
    new CGameManager.AchievementData(EAchievement.A_INeverAskedForThis, "I never asked for this", "Feel the negative side of artificial organs"),
    new CGameManager.AchievementData(EAchievement.A_BacteriaMaster, "Bacteria Master", "Beat Bacteria on mega-brutal difficulty"),
    new CGameManager.AchievementData(EAchievement.A_VirusMaster, "Virus Master", "Beat Virus disease type on mega-brutal difficulty"),
    new CGameManager.AchievementData(EAchievement.A_FungusMaster, "Fungus Master", "Beat Fungus disease type on mega-brutal difficulty"),
    new CGameManager.AchievementData(EAchievement.A_ParasiteMaster, "Parasite Master", "Beat Parasite disease type on mega-brutal difficulty"),
    new CGameManager.AchievementData(EAchievement.A_PrionMaster, "Prion Master", "Beat Prion disease type on mega-brutal difficulty"),
    new CGameManager.AchievementData(EAchievement.A_NanovirusMaster, "Nano-Virus Master", "Beat Nano-Virus disease type on mega-brutal difficulty"),
    new CGameManager.AchievementData(EAchievement.A_BioweaponMaster, "Bioweapon Master", "Beat Bioweapon disease type on mega-brutal difficulty"),
    new CGameManager.AchievementData(EAchievement.A_NeuraxMaster, "Neurax Master", "Beat Neurax Worm disease type on mega-brutal difficulty"),
    new CGameManager.AchievementData(EAchievement.A_NecroaMaster, "Necroa Master", "Beat Necroa Virus disease type on mega-brutal difficulty"),
    new CGameManager.AchievementData(EAchievement.A_NormalShuffle, "Normal Shuffle", "Win 5 times in a row on normal difficulty with the shuffle strain enabled"),
    new CGameManager.AchievementData(EAchievement.A_MegabrutalShuffle, "Mega-Brutal Shuffle", "Win 5 times in a row on mega-brutal difficulty with the shuffle strain enabled"),
    new CGameManager.AchievementData(EAchievement.A_Bingo, "Bingo!", "Win 5 times in a row on normal difficulty with the lucky dip strain enabled"),
    new CGameManager.AchievementData(EAchievement.A_LotteryWinner, "Lottery Winner!", "Win 5 times in a row on mega brutal difficulty with the lucky dip strain enabled"),
    new CGameManager.AchievementData(EAchievement.A_CompleteNipahVirus, "", "", false),
    new CGameManager.AchievementData(EAchievement.A_CompleteFrozenVirus, "", "", false),
    new CGameManager.AchievementData(EAchievement.A_CompleteArtificialOrgans, "", "", false),
    new CGameManager.AchievementData(EAchievement.A_CompleteUnknownOrigin, "", "", false),
    new CGameManager.AchievementData(EAchievement.A_apeswillrise, "Apes will Rise!", "Destroy humanity with the Simian Flu and let the apes take over"),
    new CGameManager.AchievementData(EAchievement.A_thefutureisbright, "The Future is Bright", "End the game with apes and humans living together peacefully"),
    new CGameManager.AchievementData(EAchievement.A_notjustaprettyface, "Not Just a Pretty Face", "Make every ape in the world intelligent"),
    new CGameManager.AchievementData(EAchievement.A_didimeantodothat, "Did I mean to do that?", "Reverse Simian evolution"),
    new CGameManager.AchievementData(EAchievement.A_filmfanatic, "Film Fanatic", "Recreate the setting of the Dawn of the Planet of the Apes film"),
    new CGameManager.AchievementData(EAchievement.A_familyfuture, "Family. Future.", "Create an ape colony to bring intelligent apes together"),
    new CGameManager.AchievementData(EAchievement.A_gosimianfaeces, "Go Simian-faeces", "Destroy a Gen-Sys lab"),
    new CGameManager.AchievementData(EAchievement.A_thetraveller, "The Traveller", "Move intelligent apes to a new country"),
    new CGameManager.AchievementData(EAchievement.A_evacuape, "EvacuApe", "Evade a drone attack"),
    new CGameManager.AchievementData(EAchievement.A_attackofthedrones, "Attack of the Drones", "Have a colony destroyed by a drone"),
    new CGameManager.AchievementData(EAchievement.A_apestogetherstrong, "Apes. Together. Strong.", "Gather 500,000 apes in a single country"),
    new CGameManager.AchievementData(EAchievement.A_Exterminape, "ExterminApe", "Scare humans into becoming hostile to apes"),
    new CGameManager.AchievementData(EAchievement.A_shouldntkeeppets, "Shouldn't Keep Pets", "Allow all the apes to die"),
    new CGameManager.AchievementData(EAchievement.A_anapeisforlife, "An Ape is for Life…", "…Not just for Christmas"),
    new CGameManager.AchievementData(EAchievement.A_simianmaster, "Simian Master", "Beat the Simian Flu on mega-brutal difficulty"),
    new CGameManager.AchievementData(EAchievement.A_greatescape, "Great EscApe", "Break apes out of the primate shelter"),
    new CGameManager.AchievementData(EAchievement.A_noidea, "No Idea", "Evolve 'Total Brain Death' in the first year and win"),
    new CGameManager.AchievementData(EAchievement.A_doitliketheydo, "Do it like they do", "Discover the Discovery Channel Combo"),
    new CGameManager.AchievementData(EAchievement.A_apescreed, "Ape's Creed", "Discover the Assassin Combo"),
    new CGameManager.AchievementData(EAchievement.A_likefliesround, "Like flies round…", "Discover the Fly Magnet Combo"),
    new CGameManager.AchievementData(EAchievement.A_rudeawakening, "Rude Awakening", "Discover the Rude Awakening Combo"),
    new CGameManager.AchievementData(EAchievement.A_redaperedemption, "Red Ape Redemption", "Discover the Red Ape Redemption Combo"),
    new CGameManager.AchievementData(EAchievement.A_blindgenius, "Blind Genius", "Discover the Blind Genius Combo"),
    new CGameManager.AchievementData(EAchievement.A_notnecroa, "Not Necroa", "Discover the Zombie Panic Combo"),
    new CGameManager.AchievementData(EAchievement.A_gladossayshi, "GLaDOS Says Hi", "Make a teleport attempt end in tragedy"),
    new CGameManager.AchievementData(EAchievement.A_completesantaslittlehelper, "Complete Santa's Little Helper", "Score 3 biohazards in the Santa's Little Helper scenario"),
    new CGameManager.AchievementData(EAchievement.A_completeteleportation, "Complete Teleportation", "Score 3 biohazards in the Teleportation scenario"),
    new CGameManager.AchievementData(EAchievement.A_ultimatechristmas, "Ultimate Christmas", "Discover the Ultimate Christmas Combo"),
    new CGameManager.AchievementData(EAchievement.A_onthenaughtylist, "On The Naughty List", "Put humanity on Santa's naughty list"),
    new CGameManager.AchievementData(EAchievement.A_TutorialComplete, "STEAM_TutorialComplete_Title", "STEAM_TutorialComplete_Text"),
    new CGameManager.AchievementData(EAchievement.A_MP_GeneticChallenger, "STEAM_MP_GeneticChallenger_Title", "STEAM_MP_GeneticChallenger_Text"),
    new CGameManager.AchievementData(EAchievement.A_MP_GeneticDomination, "STEAM__MP_GeneticDomination_Title", "STEAM__MP_GeneticDomination_Description"),
    new CGameManager.AchievementData(EAchievement.A_MP_BetaInfection, "STEAM__MP_BetaInfection_Title", "STEAM__MP_BetaInfection_Description"),
    new CGameManager.AchievementData(EAchievement.A_MP_NdemicInfection, "STEAM__MP_NdemicInfection_Title", "STEAM__MP_NdemicInfection_Description"),
    new CGameManager.AchievementData(EAchievement.A_MP_Brief_Acquaintance, "STEAM_MP_Brief_Acquaintance_Title", "STEAM_MP_Brief_Acquaintance_Text"),
    new CGameManager.AchievementData(EAchievement.A_MP_One_Night_Stand, "STEAM_MP_One_Night_Stand_Title", "STEAM_MP_One_Night_Stand_Text"),
    new CGameManager.AchievementData(EAchievement.A_MP_Friends_With_Benefits, "STEAM_MP_Friends_With_Benefits_Title", "STEAM_MP_Friends_With_Benefits_Text"),
    new CGameManager.AchievementData(EAchievement.A_MP_Comrade_In_Arms, "STEAM_MP_Comrade_In_Arms_Title", "STEAM_MP_Comrade_In_Arms_Text"),
    new CGameManager.AchievementData(EAchievement.A_MP_BFF, "STEAM_MP_BFF_Title", "STEAM_MP_BFF_Text"),
    new CGameManager.AchievementData(EAchievement.A_MP_Nuclear_Warfare, "STEAM_Nuclear_Warfare_Title", "STEAM_Nuclear_Warfare_Text"),
    new CGameManager.AchievementData(EAchievement.A_PublishedScenario, "STEAM_Published_Scenario_Title", "STEAM_Published_Scenario_Text"),
    new CGameManager.AchievementData(EAchievement.A_Disease_Master, "STEAM_Disease_Master_Title", "STEAM_Disease_Master_Text"),
    new CGameManager.AchievementData(EAchievement.A_Fully_Evolved, "STEAM_Fully_Evolved_Title", "STEAM_Fully_Evolved_Text"),
    new CGameManager.AchievementData(EAchievement.A_Scenario_Master, "STEAM_Scenario_Master_Title", "STEAM_Scenario_Master_Text"),
    new CGameManager.AchievementData(EAchievement.A_luckofthedevil, "Luck of the Devil", "Destroy a Templar fort with near death vampire"),
    new CGameManager.AchievementData(EAchievement.A_heartofdarkness, "Heart of Darkness", "Send your vampire into a Blood Rage"),
    new CGameManager.AchievementData(EAchievement.A_batmobile, "Batmobile", "Transform vampire into a bat and fly somewhere"),
    new CGameManager.AchievementData(EAchievement.A_vampiremaster, "Vampire Master", "Beat Shadow Plague on mega-brutal difficulty"),
    new CGameManager.AchievementData(EAchievement.A_bloodpets, "Blood Pets", "Create slaves for your vampire"),
    new CGameManager.AchievementData(EAchievement.A_purityofthechosen, "Purity of the Chosen", "Create a new vampire"),
    new CGameManager.AchievementData(EAchievement.A_homesweethome, "Home Sweet Home", "Set up a vampire lair"),
    new CGameManager.AchievementData(EAchievement.A_vanhelsingsdoom, "Van Helsing's Doom", "Destroy a Templar fort"),
    new CGameManager.AchievementData(EAchievement.A_twilightlied, "Twilight Lied", "Let Humanity discover your vampire"),
    new CGameManager.AchievementData(EAchievement.A_rockbottom, "Rock Bottom", "Consume the cavers exploring the Shadow Pool"),
    new CGameManager.AchievementData(EAchievement.A_chiroptophobia, "Chiroptophobia", "Drive out cavers before sampling the Shadow Pool"),
    new CGameManager.AchievementData(EAchievement.A_waterygrave, "Watery Grave", "Drown Templar scientists in the Shadow Pool"),
    new CGameManager.AchievementData(EAchievement.A_pusexplosion, "Pus Explosion", "Oooze pus everywhere!"),
    new CGameManager.AchievementData(EAchievement.A_wolfpack, "Wolf Pack", "Hunt down historian searching for Dracula's tomb"),
    new CGameManager.AchievementData(EAchievement.A_iamyourfather, "I am your Father", "Resurrect Count Dracula"),
    new CGameManager.AchievementData(EAchievement.A_makingamends, "Making Amends", "Help tourists repair the damage to Stonehenge"),
    new CGameManager.AchievementData(EAchievement.A_greyscale, "Greyscale", "Infect Stonehenge explorers"),
    new CGameManager.AchievementData(EAchievement.A_poweroverwhelming, "Power Overwhelming", "Consume the bones at Stonehenge"),
    new CGameManager.AchievementData(EAchievement.A_sadomasicist, "Sadomasicist", "Beat Shadow Plague with no Shadow Slaves"),
    new CGameManager.AchievementData(EAchievement.A_countcountula, "Count Countula", "Carefully count your vampire's feeds"),
    new CGameManager.AchievementData(EAchievement.A_mruniverse, "Mr. Universe", "Discover the Mr. Universe Combo"),
    new CGameManager.AchievementData(EAchievement.A_darknight, "Dark Night", "Discover the Dark Night Combo"),
    new CGameManager.AchievementData(EAchievement.A_silentbutdeadly, "Silent but Deadly", "Discover the Silent but Deadly Combo"),
    new CGameManager.AchievementData(EAchievement.A_dentistsdream, "Dentist's Dream", "Discover the Dentist's Dream Combo"),
    new CGameManager.AchievementData(EAchievement.A_uphilliceskating, "Uphill Ice Skating", "Discover the Uphill Ice Skating Combo"),
    new CGameManager.AchievementData(EAchievement.A_evilisapointofview, "Evil is a point of view", "Discover the Evil is a point of view Combo"),
    new CGameManager.AchievementData(EAchievement.A_bloodtrumpsall, "Blood Trumps All", "Discover the Blood Trumps All Combo"),
    new CGameManager.AchievementData(EAchievement.A_carpejugulum, "Carpe Jugulum", "Discover the Carpe Jugulum Combo"),
    new CGameManager.AchievementData(EAchievement.A_welcometohellmouth, "Welcome to Hellmouth", "Discover the Welcome to Hellmouth Combo"),
    new CGameManager.AchievementData(EAchievement.A_diamondskin, "Diamond Skin", "Discover the Diamond Skin Combo"),
    new CGameManager.AchievementData(EAchievement.A_batcave, "Bat Cave", "Discover the Bat Cave Combo"),
    new CGameManager.AchievementData(EAchievement.A_essentialvitamins, "Essential Vitamins", "Discover the Essential Vitamins Combo"),
    new CGameManager.AchievementData(EAchievement.A_plaguedogs, "Plague Dogs", "Discover the Plague Dogs Combo"),
    new CGameManager.AchievementData(EAchievement.A_NoBrexit, "No Brexit", "Stop Britain leaving the EU"),
    new CGameManager.AchievementData(EAchievement.A_BrutalBrexit, "Brutal Brexit", "Put Britain on the road to madness when it leaves the EU"),
    new CGameManager.AchievementData(EAchievement.A_HardBrexit, "Hard Brexit", "Encourage Britain to have a 'hard' Brexit"),
    new CGameManager.AchievementData(EAchievement.A_SoftBrexit, "Soft Brexit", "Help Britain softly leave the EU"),
    new CGameManager.AchievementData(EAchievement.A_completemaddcow, "Complete Mad Cow Disease", "Score 3 biohazards in the Mad Cow scenario"),
    new CGameManager.AchievementData(EAchievement.A_sushicrisis, "Sushi Crisis", "Start a Sushi Crisis in Japan"),
    new CGameManager.AchievementData(EAchievement.A_completeflightclub, "Complete Flight Club", "Score 3 biohazards in the Flight Club scenario"),
    new CGameManager.AchievementData(EAchievement.A_completewhereiseveryone, "Complete Where is Everyone?", "Score 3 biohazards in the Where is Everyone? scenario"),
    new CGameManager.AchievementData(EAchievement.A_kindofabigdeal, "Kind Of A Big Deal", "Discover the Poker board game combo"),
    new CGameManager.AchievementData(EAchievement.A_acoltclassic, "A Colt Classic", "Discover the Steeplechase board game combo"),
    new CGameManager.AchievementData(EAchievement.A_infectingtabletops, "Infecting Tabletops", "Discover a very familiar board game combo"),
    new CGameManager.AchievementData(EAchievement.A_sinkingfeelings, "Sinking Feelings", "Discover the Battleship board game combo"),
    new CGameManager.AchievementData(EAchievement.A_partyhard, "Party Hard", "Discover a compulsory enjoyment board game combo"),
    new CGameManager.AchievementData(EAchievement.A_takingrisks, "Taking Risks", "Discover the Risk board game combo"),
    new CGameManager.AchievementData(EAchievement.A_catanyoubelieveit, "Catan You Believe It?", "Discover the Settlers Of Catan board game combo"),
    new CGameManager.AchievementData(EAchievement.A_spellingoutsuccess, "Spelling Out Success", "Discover the Scrabble board game combo"),
    new CGameManager.AchievementData(EAchievement.A_killercombo, "Killer Combo", "Discover the Cluedo/Clue board game combo"),
    new CGameManager.AchievementData(EAchievement.A_anythingbuttrivial, "Anything But Trivial", "Discover the Trivial Pursuit board game combo"),
    new CGameManager.AchievementData(EAchievement.A_welcometothejungle, "Welcome To The Jungle!", "Discover a cursed board game combo"),
    new CGameManager.AchievementData(EAchievement.A_plagueopoly, "Plague-opoly", "Discover the Monopoly board game combo"),
    new CGameManager.AchievementData(EAchievement.A_awkward, "Awkward!", "Discover an inappropriate board game combo"),
    new CGameManager.AchievementData(EAchievement.A_completeultimateboardgames, "Complete Ultimate Board Games", "Score three biohazards in the Ultimate Board Games scenario"),
    new CGameManager.AchievementData(EAchievement.A_completesciencedenial, "Complete Science Denial", "Score three biohazards in the Science Denial scenario"),
    new CGameManager.AchievementData(EAchievement.A_thecureisalie, "The Cure Is A Lie", "Inspire an anti-cure protest"),
    new CGameManager.AchievementData(EAchievement.A_makingHeatWaves, "Making 'heat' waves", "Convince the world there is no environment crisis"),
    new CGameManager.AchievementData(EAchievement.A_hackJob, "Hack Job", "Convince the world that the elections were rigged"),
    new CGameManager.AchievementData(EAchievement.A_gamegate, "Gamergate", "Please don't review bomb us"),
    new CGameManager.AchievementData(EAchievement.A_generationalTensions, "Generational tension", "Convince the world. Divide the world."),
    new CGameManager.AchievementData(EAchievement.A_district9, "District 9", "Convince the world that the newcomers don't belong"),
    new CGameManager.AchievementData(EAchievement.A_independenceDay, "Independence Day", "Convince the world with an otherworldly threat"),
    new CGameManager.AchievementData(EAchievement.A_lochNessMonster, "Loch Ness Monster", "Convince the world of a legend in the loch"),
    new CGameManager.AchievementData(EAchievement.A_tooCuteToLie, "Too cute to lie", "Convince the world it's not feline fine"),
    new CGameManager.AchievementData(EAchievement.A_filterBubble, "Filter bubble", "Beat the Fake News Scenario on Normal"),
    new CGameManager.AchievementData(EAchievement.A_postTruthSociety, "Post-truth society", "Beat the Fake News Scenario on Brutal"),
    new CGameManager.AchievementData(EAchievement.A_weLoveFactCheckers, "We love fact checkers", "Get Fact Checked"),
    new CGameManager.AchievementData(EAchievement.A_onTheFence, "On the fence", "Tear the world in two with indecision"),
    new CGameManager.AchievementData(EAchievement.A_bacteriacured, "Bacteria Cured!", "Save the world from Bacteria on any difficulty"),
    new CGameManager.AchievementData(EAchievement.A_viruscured, "Virus Cured!", "Save the world from Virus on any difficulty"),
    new CGameManager.AchievementData(EAchievement.A_parasitecured, "Parasite Cured!", "Save the world from Parasite on any difficulty"),
    new CGameManager.AchievementData(EAchievement.A_funguscured, "Fungus Cured!", "Save the world from Fungus on any difficulty"),
    new CGameManager.AchievementData(EAchievement.A_prioncured, "Prion Cured!", "Save the world from Prion on any difficulty"),
    new CGameManager.AchievementData(EAchievement.A_nanoviruscured, "Nano-Virus Cured!", "Save the world from Nano-Virus on any difficulty"),
    new CGameManager.AchievementData(EAchievement.A_bioweaponcured, "Bio-Weapon Cured!", "Save the world from Bio-Weapon on any difficulty"),
    new CGameManager.AchievementData(EAchievement.A_antivaxxer, "Anti Vaxxer", "Save the world without using a vaccine"),
    new CGameManager.AchievementData(EAchievement.A_doomsdaysave, "Doomsday Save", "Save the world with less than 5% of the world's population left"),
    new CGameManager.AchievementData(EAchievement.A_iwillfindyou, "I will find you", "Investigate a new disease outbreak"),
    new CGameManager.AchievementData(EAchievement.A_shutdowneverything, "Shut down everything", "Do as Madagascar does"),
    new CGameManager.AchievementData(EAchievement.A_inittogether, "In it together", "Initiative combo: In it together"),
    new CGameManager.AchievementData(EAchievement.A_humanswerentmeanttofly, "Humans weren't meant to fly", "Initiative combo: Humans weren't meant to fly"),
    new CGameManager.AchievementData(EAchievement.A_wereheretohelp, "We're here to help", "Initiative combo: We're here to help"),
    new CGameManager.AchievementData(EAchievement.A_reportthyneighbour, "Report thy neighbour", "Initiative combo: Report thy neighbour"),
    new CGameManager.AchievementData(EAchievement.A_happyfrogs, "Happy Frogs", "Initiative combo: Happy Frogs"),
    new CGameManager.AchievementData(EAchievement.A_rulesareforlosers, "Rules are for losers", "Initiative combo: Rules are for losers"),
    new CGameManager.AchievementData(EAchievement.A_weaponx, "Weapon X", "Initiative combo: Weapon X"),
    new CGameManager.AchievementData(EAchievement.None, "", "", false)
  };
  public static Dictionary<string, int> scenarioConstList;
  public static List<string> constedScenarioList;
  public static List<CScenarioScore> scenarioScores;
  public static List<CScenarioScore> b30;
  public static double oldPotential;
  public static double newPotential;
  public static bool constGot;
  public static int messageReceived;
  public static Dictionary<string, CGameManager.ScenarioUnlockCondition> scenarioUnlockConditionScore;
  public static Dictionary<string, List<CGameManager.ScenarioUnlockCondition>> scenarioUnlockConditionOverride;
  public static CSplashScreenVideo video;
  public static bool playOnlineVideo;
  public static string onlineVideoLink;
  public static string pendingScenarioName;
  public static string FilterScenarioLevel;
  public static string FilterScenarioType;
  public static string FilterScenarioComplete;
  public static string annoyerListGeneral;
  public static bool canMatchAnnoyer;
  public static List<string> annoyerList;
  public static CGameManager.EBlockType blockType;
  public static bool cheatDetected;
  public static bool spaceTime;
  public static int spaceTimeRate;
  public static bool showConst;
  public static int oldScore;
  public static bool sortByScore;
  public static Dictionary<string, int> scenarioScoreDict;
  public static bool returnFromScenario;
  public static Dictionary<string, string> scenarioNameTitlePair;
  public static Dictionary<string, string> federalScenarioAuthorList;
  public static Dictionary<string, int> scenarioUnlockPotential;
  public static string scenarioSort;
  public static string scenarioShow;
  public static Dictionary<string, string> scenarioName;
  public static double currentPotential;
  public static int currentScore;
  public static JSONNode scenarioData;
  public static List<string> federalScenarioNames;
  public static Dictionary<string, string> scenarioDifficultyRaw;
  public static bool scenarioListGot;
  public static bool prepareDone;
  public static int toDownload;
  public static int downloaded;
  public static int toExtract;
  public static int extracted;
  public static Dictionary<string, string> scenarioPack;
  public static Dictionary<string, int> scenarioDate;
  public static Dictionary<string, string> scenarioPackDisplayName;
  public static bool InvertSortType;
  public static string ScenarioSearchKeyword;
  public static string federalServerAddress;

  public static string GetGameModeName(IGame.GameType gameType)
  {
    switch (gameType)
    {
      case IGame.GameType.Classic:
        return "FE_Single_Player";
      case IGame.GameType.VersusMP:
        return "MP_Mode_Screen_Vs_Game_Mode_Title";
      case IGame.GameType.CoopMP:
        return "MP_Mode_Screen_Co_Op_Game_Mode_Title";
      case IGame.GameType.Cure:
        return "FE_GAME_MODE_CURE";
      default:
        return "FE_Single_Player";
    }
  }

  public static List<int> GetCoopDifficulties()
  {
    return new List<int>() { 0, 1, 2 };
  }

  public static string GetDiseaseName(Disease.EDiseaseType diseaseType)
  {
    return CGameManager.DiseaseNames.ContainsKey(diseaseType) ? CGameManager.DiseaseNames[diseaseType] : CGameManager.DiseaseNames[Disease.EDiseaseType.BACTERIA];
  }

  public static string GetDiseaseDescriptionMP(Disease.EDiseaseType diseaseType)
  {
    return CGameManager.DiseaseDescriptionsMP.ContainsKey(diseaseType) ? CGameManager.DiseaseDescriptionsMP[diseaseType] : CGameManager.DiseaseDescriptionsMP[Disease.EDiseaseType.BACTERIA];
  }

  public static string GetDiseaseSprite(Disease.EDiseaseType diseaseType)
  {
    string str = diseaseType.ToString();
    return str.Substring(0, 1).ToUpper() + str.Substring(1).ToLower();
  }

  public static string GetGeneBackgroundSprite(string geneId, bool isLocked, int playerId = 0)
  {
    switch (geneId)
    {
      case "_empty":
        return playerId != 1 ? "MP_Gene_Base_Unselected" : "MP_Gene_Base_Unselected_P2";
      case "_indev":
        return "Gene_Small_Base_In_Dev";
      default:
        if (isLocked)
          return "Gene_Small_Base_Locked";
        return playerId != 1 ? "Gene_Small_Base_Selected" : "Gene_Small_Human_Selected";
    }
  }

  public static event CGameManager.OnGameSpeedChangeEvent onGameSpeedChange;

  public static List<ScenarioInformation> OfficialScenarios { get; private set; }

  public static bool IsMultiplayerGame
  {
    get
    {
      return CGameManager.gameType == IGame.GameType.VersusMP || CGameManager.gameType == IGame.GameType.CoopMP;
    }
  }

  public static bool IsVersusMPGame => CGameManager.gameType == IGame.GameType.VersusMP;

  public static bool IsCoopMPGame => CGameManager.gameType == IGame.GameType.CoopMP;

  public static bool IsAIGame
  {
    get
    {
      if (CGameManager.gameType == IGame.GameType.VersusMP && ((MultiplayerGame) CGameManager.game).IsAIGame)
        return true;
      return CGameManager.gameType == IGame.GameType.CoopMP && ((CooperativeGame) CGameManager.game).IsAIGame;
    }
  }

  public static bool IsTutorialGame
  {
    get
    {
      return CGameManager.gameType == IGame.GameType.Tutorial || CGameManager.gameType == IGame.GameType.CureTutorial;
    }
  }

  public static bool IsCureTutorialGame => CGameManager.gameType == IGame.GameType.CureTutorial;

  public static bool IsCureGame
  {
    get
    {
      return CGameManager.gameType == IGame.GameType.Cure || CGameManager.gameType == IGame.GameType.CureTutorial;
    }
  }

  public static void Initialise()
  {
    CGameManager.game = (IGame) null;
    ISaves[] objectsOfType1 = UnityEngine.Object.FindObjectsOfType<ISaves>();
    for (int index = 0; index < objectsOfType1.Length; ++index)
    {
      if (objectsOfType1[index].IsAvailable())
        CGameManager.saves = objectsOfType1[index];
    }
    IScores[] objectsOfType2 = UnityEngine.Object.FindObjectsOfType<IScores>();
    for (int index = 0; index < objectsOfType2.Length; ++index)
    {
      if (objectsOfType2[index].IsAvailable())
        CGameManager.scores = objectsOfType2[index];
    }
    if ((bool) (UnityEngine.Object) CGameManager.scores)
      CGameManager.scores.Initialise();
    else
      Debug.LogError((object) "Could not locate IScores component");
    CGameManager.currentGameDate = DateTime.MinValue;
    CGameManager.gameType = IGame.GameType.Invalid;
    CGameManager.localPlayerInfo = (IPlayerInfo) null;
    CGameManager.pauseHud = false;
    CGameManager.pauseInterface = false;
    CGameManager.pausePopup = false;
    CGameManager.paused = false;
    CGameManager.canPlaySFX = true;
    CGameManager.dynamicNewsText = string.Empty;
    CGameManager.dynamicPopupText = string.Empty;
    CGameManager.lastSpeed = 1;
    CGameManager.gameStartTime = 0.0f;
    CGameManager.gameTimeDiseaseType = new Disease.EDiseaseType?(Disease.EDiseaseType.BACTERIA);
    CGameManager.OfficialScenarios = new List<ScenarioInformation>();
    CGameManager.LoadOfficialScenarios();
    CInterfaceManager.instance.Initialise();
  }

  public static void GameUpdate()
  {
    if (!((UnityEngine.Object) CGameManager.game != (UnityEngine.Object) null))
      return;
    CGameManager.game.GameUpdate();
  }

  public static void GameSpeedChanged(int speed, bool hasChanged = true, int mySpeed = 0, int oppSpeed = 0)
  {
    if (speed > 3 && (UnityEngine.Object) CGameManager.game != (UnityEngine.Object) null && !CGameManager.game.IsReplayActive && !CGameManager.IsFederalScenario("PISMG"))
    {
      CGameManager.game.EndGame(IGame.EndGameReason.DESYNC);
    }
    else
    {
      if (CGameManager.onGameSpeedChange != null & hasChanged)
        CGameManager.onGameSpeedChange();
      CInterfaceManager.instance.SetSpeedState(speed, mySpeed, oppSpeed);
    }
  }

  public static void CreateGame(Scenario scenario = null, Disease.EDiseaseType overrideDiseaseType = Disease.EDiseaseType.BACTERIA)
  {
    CGameManager.localPlayerInfo = CNetworkManager.network.LocalPlayerInfo;
    GameObject gameObject1 = GameObject.Find("StandardGame");
    if ((UnityEngine.Object) gameObject1 == (UnityEngine.Object) null)
    {
      GameObject gameObject2 = new GameObject();
      gameObject2.name = "StandardGame";
      CGameManager.game = (IGame) gameObject2.AddComponent<StandardGame>();
    }
    else
      CGameManager.game = (IGame) gameObject1.GetComponent<StandardGame>();
    if ((UnityEngine.Object) CGameManager.game != (UnityEngine.Object) null)
    {
      CGameManager.game.diseaseColourSets = new DiseaseColourSet[Main.instance.diseaseColourSets.Length];
      Array.Copy((Array) Main.instance.diseaseColourSets, (Array) CGameManager.game.diseaseColourSets, CGameManager.game.diseaseColourSets.Length);
      CGameManager.game.neuraxDiseaseColourSets = new DiseaseColourSet[Main.instance.neuraxDiseaseColourSets.Length];
      Array.Copy((Array) Main.instance.neuraxDiseaseColourSets, (Array) CGameManager.game.neuraxDiseaseColourSets, CGameManager.game.neuraxDiseaseColourSets.Length);
      CGameManager.game.zombieDiseaseColourSets = new DiseaseColourSet[Main.instance.zombieDiseaseColourSets.Length];
      Array.Copy((Array) Main.instance.zombieDiseaseColourSets, (Array) CGameManager.game.zombieDiseaseColourSets, CGameManager.game.zombieDiseaseColourSets.Length);
      CGameManager.game.simianDiseaseColourSets = new DiseaseColourSet[Main.instance.simianDiseaseColourSets.Length];
      Array.Copy((Array) Main.instance.simianDiseaseColourSets, (Array) CGameManager.game.simianDiseaseColourSets, CGameManager.game.simianDiseaseColourSets.Length);
      CGameManager.game.vampireDiseaseColourSets = new DiseaseColourSet[Main.instance.vampireDiseaseColourSets.Length];
      Array.Copy((Array) Main.instance.vampireDiseaseColourSets, (Array) CGameManager.game.vampireDiseaseColourSets, CGameManager.game.vampireDiseaseColourSets.Length);
      CGameManager.game.cureDiseaseColourSets = new DiseaseColourSet[Main.instance.cureDiseaseColourSets.Length];
      Array.Copy((Array) Main.instance.cureDiseaseColourSets, (Array) CGameManager.game.cureDiseaseColourSets, CGameManager.game.cureDiseaseColourSets.Length);
      CGameManager.game.Initialise();
      CGameManager.game.CreateGame(scenario, overrideDiseaseType);
      CGameManager.GameSpeedChanged(1);
      CInterfaceManager.instance.SetupOfflineGame();
    }
    else
      Debug.LogError((object) "IGame not valid for standard game");
    if (!CGameManager.IsTutorialGame)
      return;
    COptionsManager.instance.InterfaceType = EInterfaceType.Full;
  }

  public static IGame InitialiseGame(IGame.GameType newGameType)
  {
    CGameManager.localPlayerInfo = CNetworkManager.network.LocalPlayerInfo;
    CGameManager.lastGameType = CGameManager.gameType = newGameType;
    GameObject gameObject1 = GameObject.Find(CGameManager.gameType.ToString() + "Game");
    if ((UnityEngine.Object) gameObject1 == (UnityEngine.Object) null)
    {
      GameObject gameObject2 = new GameObject();
      gameObject2.name = CGameManager.gameType.ToString() + "Game";
      switch (CGameManager.gameType)
      {
        case IGame.GameType.VersusMP:
          CGameManager.game = (IGame) gameObject2.AddComponent<MultiplayerGame>();
          break;
        case IGame.GameType.CoopMP:
          CGameManager.game = (IGame) gameObject2.AddComponent<CooperativeGame>();
          break;
      }
    }
    else
    {
      switch (CGameManager.gameType)
      {
        case IGame.GameType.VersusMP:
          CGameManager.game = (IGame) gameObject1.GetComponent<MultiplayerGame>();
          break;
        case IGame.GameType.CoopMP:
          CGameManager.game = (IGame) gameObject1.GetComponent<CooperativeGame>();
          break;
      }
    }
    if ((UnityEngine.Object) CGameManager.game != (UnityEngine.Object) null)
    {
      CGameManager.game.diseaseColourSets = Main.instance.diseaseColourSets;
      CGameManager.game.neuraxDiseaseColourSets = Main.instance.neuraxDiseaseColourSets;
      CGameManager.game.zombieDiseaseColourSets = Main.instance.zombieDiseaseColourSets;
      CGameManager.game.simianDiseaseColourSets = Main.instance.simianDiseaseColourSets;
      CGameManager.game.vampireDiseaseColourSets = Main.instance.vampireDiseaseColourSets;
      CGameManager.game.cureDiseaseColourSets = Main.instance.cureDiseaseColourSets;
      CGameManager.game.Initialise();
    }
    else
      Debug.LogError((object) "IGame not valid for multiplayer game");
    return CGameManager.game;
  }

  public static bool LoadGame(int slotID, ref bool customGame)
  {
    CGameManager.canPlaySFX = false;
    CGameManager.ClearGame();
    DynamicMusic.instance.FadeOut();
    ISaves.SaveMetaData metaData = CGameManager.saves.GetMetaData(slotID);
    if (metaData == null)
    {
      Debug.LogError((object) ("Meta data null for slot: " + (object) slotID));
      return false;
    }
    CGameManager.gameType = metaData.gameType;
    Scenario scenario = (Scenario) null;
    if (!string.IsNullOrEmpty(metaData.scenario))
    {
      if (metaData.gameType == IGame.GameType.Custom || metaData.gameType == IGame.GameType.Cure && metaData.scenario.Contains("PIFCURE"))
      {
        string scenario1 = metaData.scenario;
        if (scenario1.StartsWith("NDEMIC:"))
        {
          long ndemicID = long.Parse(scenario1.Substring(7));
          CUIManager.instance.StartCoroutine(CustomScenarioCache.Load(ndemicID, (Action<Scenario>) (scen =>
          {
            scen.scenarioInformation.id = string.Concat((object) ndemicID);
            scen.eventData = scen.ReadText("events");
            if (scen.iconsLoading > 0)
            {
              Scenario.IconsLoadedEvent callback = (Scenario.IconsLoadedEvent) null;
              callback = (Scenario.IconsLoadedEvent) (() =>
              {
                scen.onIconsLoaded -= callback;
                CGameManager.FinishLoadGame(scen, slotID);
              });
              scen.onIconsLoaded += callback;
            }
            CGameManager.FinishLoadGame(scen, slotID);
          })));
          return true;
        }
        if (CSLocalUGCHandler.LocalExists(scenario1))
        {
          scenario = new CSLocalUGCHandler().LocalCreateScenario(scenario1);
          scenario.scenarioInformation.id = scenario1;
          scenario.eventData = scenario.ReadText("events");
          if (scenario.iconsLoading > 0)
          {
            Scenario.IconsLoadedEvent callback = (Scenario.IconsLoadedEvent) null;
            callback = (Scenario.IconsLoadedEvent) (() =>
            {
              scenario.onIconsLoaded -= callback;
              CGameManager.FinishLoadGame(scenario, slotID);
            });
            scenario.onIconsLoaded += callback;
          }
          else
            CGameManager.FinishLoadGame(scenario, slotID);
          return true;
        }
        ulong publishID = metaData.publishID;
        if (publishID == 0UL)
          return false;
        SteamUGCHandler steamUGCHandler = new SteamUGCHandler();
        string[] strArray = new string[8];
        strArray[0] = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        strArray[1] = Path.DirectorySeparatorChar.ToString();
        strArray[2] = "Ndemic Creations";
        char directorySeparatorChar = Path.DirectorySeparatorChar;
        strArray[3] = directorySeparatorChar.ToString();
        strArray[4] = "Plague Inc. Evolved";
        directorySeparatorChar = Path.DirectorySeparatorChar;
        strArray[5] = directorySeparatorChar.ToString();
        strArray[6] = "Scenario Cache";
        directorySeparatorChar = Path.DirectorySeparatorChar;
        strArray[7] = directorySeparatorChar.ToString();
        string localCacheDirectory = string.Concat(strArray);
        steamUGCHandler.Initialise(localCacheDirectory);
        steamUGCHandler.onUGCQueryError = (SteamUGCHandler.OnUGCQueryError) (error =>
        {
          if ((UnityEngine.Object) CGameManager.game != (UnityEngine.Object) null)
          {
            Debug.LogError((object) ("UGC QUERY ERROR WHILST GAME IN PROGRESS, SUPPRESSING POPUP: " + error));
          }
          else
          {
            CConfirmOverlay redConfirmOverlay = CUIManager.instance.redConfirmOverlay;
            redConfirmOverlay.ShowLocalised("UGC_Steam_Error_Title", "UGC_Steam_Error_Text", buttonB: "OK");
            redConfirmOverlay.Body = redConfirmOverlay.Body.Replace("%error", error);
          }
        });
        CGameManager.LoadScenarioRemote(steamUGCHandler, publishID, (Action<Scenario>) (loadedScenario =>
        {
          loadedScenario.scenarioInformation.id = publishID.ToString();
          loadedScenario.eventData = loadedScenario.ReadText("events");
          if (loadedScenario.iconsLoading > 0)
          {
            Scenario.IconsLoadedEvent callback = (Scenario.IconsLoadedEvent) null;
            callback = (Scenario.IconsLoadedEvent) (() =>
            {
              loadedScenario.onIconsLoaded -= callback;
              CGameManager.FinishLoadGame(loadedScenario, slotID);
            });
            loadedScenario.onIconsLoaded += callback;
          }
          else
            CGameManager.FinishLoadGame(loadedScenario, slotID);
          CUIManager.instance.HideOverlay("Scenario_Loading_Overlay");
        }), false);
        steamUGCHandler.QuerySelectedUGCContent((IList<ulong>) new List<ulong>()
        {
          publishID
        });
        customGame = true;
        return false;
      }
      if (metaData.gameType == IGame.GameType.Official || metaData.gameType == IGame.GameType.Cure && !metaData.scenario.Contains("PIFCURE"))
      {
        ScenarioInformation officialScenario = CGameManager.GetOfficialScenario(metaData.scenario);
        if (officialScenario == null)
        {
          Debug.LogError((object) ("Could not find official scenario '" + metaData.scenario + "'"));
          return false;
        }
        scenario = Scenario.LoadScenario(officialScenario.filePath, true, false);
        scenario.scenarioInformation = officialScenario;
        scenario.isOfficial = true;
      }
    }
    CGameManager.FinishLoadGame(scenario, slotID);
    return true;
  }

  private static void CancelLoading(CGameOverlay overlay, SteamUGCHandler handler)
  {
    handler.CancelDownloads();
    CUIManager.instance.HideOverlay(overlay);
  }

  private static void UpdateProgressBar(
    CConfirmOverlay overlay,
    SteamUGCHandler handler,
    ulong fileID)
  {
    if (!(bool) (UnityEngine.Object) overlay.progressBar)
      return;
    float? nullable = handler.ItemDownloadProgress(fileID);
    overlay.progressBar.value = nullable.HasValue ? nullable.Value : 0.0f;
  }

  private static void CompleteDownload(
    SteamUGCHandler steamUGCHandler,
    ulong fileID,
    Action<Scenario> callback,
    bool showDownloadProgress,
    string folderPath,
    ulong forPublishFileID,
    bool isLegacy)
  {
    Debug.Log((object) ("INFO: Download Finished: " + folderPath + " publishID: " + (object) forPublishFileID + " isLegacy: " + isLegacy.ToString()));
    if ((long) fileID != (long) forPublishFileID)
      return;
    ExportData export = ExportData.GetFromPath(folderPath, isLegacy);
    if (export != null)
    {
      CUIManager.instance.HideOverlay("Red_Confirm_Overlay");
      if (callback == null)
        return;
      if (export.iconsLoading > 0)
      {
        ExportData.IconsLoadedEvent iconCallback = (ExportData.IconsLoadedEvent) null;
        iconCallback = (ExportData.IconsLoadedEvent) (() =>
        {
          export.onIconsLoaded -= iconCallback;
          callback(export.GetScenario());
        });
        export.onIconsLoaded += iconCallback;
      }
      else
        callback(export.GetScenario());
    }
    else
    {
      CUIManager instance = CUIManager.instance;
      if (showDownloadProgress)
        instance.HideOverlay("Scenario_Loading_Overlay");
      CConfirmOverlay overlay = instance.GetOverlay("MessageModal_Overlay") as CConfirmOverlay;
      if ((UnityEngine.Object) overlay != (UnityEngine.Object) null)
      {
        overlay.ShowLocalised("FE_Unpublished_Warning_Title", "FE_Unpublished_Warning_Text");
        instance.HideOverlay("Red_Confirm_Overlay");
      }
      else
        Debug.LogError((object) "Unable to show message modal");
    }
  }

  public static void LoadScenarioRemote(
    SteamUGCHandler steamUGCHandler,
    ulong fileID,
    Action<Scenario> callback,
    bool showDownloadProgress = true)
  {
    if (steamUGCHandler == null)
      Debug.LogError((object) "LOAD: Unable to load as steam handler is null");
    if (showDownloadProgress)
    {
      CConfirmOverlay loadingOverlay = CUIManager.instance.GetOverlay("Scenario_Loading_Overlay") as CConfirmOverlay;
      loadingOverlay.ShowLocalised("FE_PleaseWait", "FE_Scenario_Information", "MP_Cancel", pressA: (CConfirmOverlay.PressDelegate) (() => CGameManager.CancelLoading((CGameOverlay) loadingOverlay, steamUGCHandler)), update: (CConfirmOverlay.PressDelegate) (() => CGameManager.UpdateProgressBar(loadingOverlay, steamUGCHandler, fileID)));
    }
    SteamUGCHandler.OnUGCItemDownloadComplete onDownloadCompleteCallback = (SteamUGCHandler.OnUGCItemDownloadComplete) ((folderPath, forPublishFileID, isLegacy) => CGameManager.CompleteDownload(steamUGCHandler, fileID, callback, showDownloadProgress, folderPath, forPublishFileID, isLegacy));
    if (steamUGCHandler.DownloadFile(fileID, 0U, onDownloadCompleteCallback))
      return;
    CUIManager.instance.HideOverlay("Scenario_Loading_Overlay");
    CConfirmOverlay overlay = CUIManager.instance.GetOverlay("Red_Confirm_Overlay_Single") as CConfirmOverlay;
    if (!((UnityEngine.Object) overlay != (UnityEngine.Object) null))
      return;
    overlay.ShowLocalised("UGC_Unavailable_Title", "UGC_ScenarioData_Unavailable", "OK");
  }

  private static void FinishLoadGame(Scenario scenario, int slotID)
  {
    CGameManager.CreateGame(scenario);
    CGameManager.game.RecoverGameState(CGameManager.saves.LoadGame(slotID), CGameManager.saves.LoadReplayData(slotID));
    CGameManager.game.LoadScenarioStrings();
    CGameManager.saves.LoadInfectionTextures(slotID);
    CGameManager.game.ResumeGame();
    GC.Collect();
    CGameManager.RecordGameTimeStart(CGameManager.localPlayerInfo.disease.diseaseType);
  }

  public static void RecordGameTimeStart(Disease.EDiseaseType diseaseType)
  {
    CGameManager.gameStartTime = Time.realtimeSinceStartup;
    CGameManager.gameTimeDiseaseType = new Disease.EDiseaseType?(diseaseType);
  }

  public static void RecordGameTimeEnd()
  {
    if (!CGameManager.gameTimeDiseaseType.HasValue)
      return;
    Analytics.Event("Play Time " + (object) CGameManager.gameTimeDiseaseType, Mathf.FloorToInt(Time.realtimeSinceStartup - CGameManager.gameStartTime).ToString());
    CGameManager.gameTimeDiseaseType = new Disease.EDiseaseType?();
  }

  public static void ClearGame()
  {
    CGameManager.spaceTime = false;
    Debug.Log((object) "Clear Game Called");
    CInterfaceManager.instance.Cleanup();
    CGameManager.RecordGameTimeEnd();
    if ((UnityEngine.Object) CGameManager.game != (UnityEngine.Object) null)
      CGameManager.game.SetSpeed(0);
    DynamicMusic.instance.playerDisease = (Disease) null;
    if (World.instance != null)
      World.instance.Clear();
    MPPlayerData.Clear();
    GameObject gameObject1 = GameObject.Find(IGame.GameType.VersusMP.ToString() + "Game");
    if ((UnityEngine.Object) gameObject1 != (UnityEngine.Object) null)
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) gameObject1);
    GameObject gameObject2 = GameObject.Find(IGame.GameType.CoopMP.ToString() + "Game");
    if ((UnityEngine.Object) gameObject2 != (UnityEngine.Object) null)
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) gameObject2);
    INetwork network = CNetworkManager.network;
    network.Terminate();
    network.Initialize();
    if (CGameManager.localPlayerInfo != null)
      CGameManager.localPlayerInfo.disease = (Disease) null;
    CGameManager.game = (IGame) null;
    CGameManager.gameType = IGame.GameType.Invalid;
    CGameManager.lastSpeed = 1;
    CGameManager.paused = false;
    CLocalisationManager.ClearCustomLocalisation();
    GC.Collect();
  }

  public static void SetPaused(bool state, bool forced = false)
  {
    if ((UnityEngine.Object) CGameManager.game == (UnityEngine.Object) null)
      return;
    if (state && !CGameManager.paused | forced)
    {
      if (!forced && !CGameManager.pauseInterface && !CGameManager.pauseHud && (!CGameManager.pausePopup || !COptionsManager.instance.mbAutoPause))
        return;
      CGameManager.paused = true;
      CGameManager.game.SetSpeed(0);
    }
    else
    {
      if (state || !(CGameManager.paused | forced) || !forced && (CGameManager.pauseInterface || CGameManager.pauseHud || CGameManager.pausePopup && COptionsManager.instance.mbAutoPause))
        return;
      CGameManager.paused = false;
      CGameManager.game.SetSpeed(CGameManager.lastSpeed);
    }
  }

  public static bool AwardAchievement(EAchievement? achievement)
  {
    if (CGameManager.gameType == IGame.GameType.Custom || CGameManager.usingDiseaseX || !achievement.HasValue || CNetworkManager.network.LocalPlayerInfo.GetAchievement(achievement.Value))
      return false;
    CNetworkManager.network.LocalPlayerInfo.SetAchievement(achievement.Value);
    CInterfaceManager.instance.ShowAchievement(achievement.Value);
    return true;
  }

  public static Disease.EDiseaseType GetDiseaseType(string plagueName)
  {
    try
    {
      return (Disease.EDiseaseType) Enum.Parse(typeof (Disease.EDiseaseType), plagueName, true);
    }
    catch (ArgumentException ex)
    {
      Debug.Log((object) ex.Message);
    }
    switch (plagueName)
    {
      case "zombie":
        return Disease.EDiseaseType.NECROA;
      case "escaped_bio_weapon":
        return Disease.EDiseaseType.BIO_WEAPON;
      case "rogue_nanobot":
        return Disease.EDiseaseType.NANO_VIRUS;
      default:
        return Disease.EDiseaseType.BACTERIA;
    }
  }

  public static string GetDiseaseNameLoc(Disease.ECureScenario diseaseType)
  {
    return CLocalisationManager.GetText(CGameManager.GetDiseaseName(CGameManager.GetCureDiseaseType(diseaseType)));
  }

  public static string GetDiseaseNameLoc(Disease.EDiseaseType diseaseType)
  {
    return CLocalisationManager.GetText(CGameManager.GetDiseaseName(diseaseType));
  }

  public static string GetDiseaseDescriptionMPLoc(Disease.EDiseaseType diseaseType)
  {
    Debug.Log((object) ("----GetDiseaseDescriptionMP(" + (object) diseaseType + "):" + CGameManager.GetDiseaseDescriptionMP(diseaseType)));
    return CLocalisationManager.GetText(CGameManager.GetDiseaseDescriptionMP(diseaseType));
  }

  public static Disease.EDiseaseType? GetDiseaseNext(Disease.EDiseaseType type)
  {
    int num = CGameManager.DiseaseUnlockOrder.IndexOf(type);
    return num >= CGameManager.DiseaseUnlockOrder.Count - 1 || num < 0 ? new Disease.EDiseaseType?() : new Disease.EDiseaseType?(CGameManager.DiseaseUnlockOrder[num + 1]);
  }

  public static Disease.ECureScenario? GetCureNext(Disease.ECureScenario type)
  {
    int num = CGameManager.CureUnlockOrder.IndexOf(type);
    return num >= CGameManager.CureUnlockOrder.Count - 1 || num < 0 ? new Disease.ECureScenario?() : new Disease.ECureScenario?(CGameManager.CureUnlockOrder[num + 1]);
  }

  public static string GetScenarioDiseaseDescriptionLocked(Disease.EDiseaseType type)
  {
    return CLocalisationManager.GetText("FE_Speedrun_Locked_Message").Replace("%s", CGameManager.GetDiseaseNameLoc(type));
  }

  public static Disease.EDiseaseType? GetDiseasePrev(Disease.EDiseaseType type)
  {
    int num = CGameManager.DiseaseUnlockOrder.IndexOf(type);
    return num <= 0 ? new Disease.EDiseaseType?() : new Disease.EDiseaseType?(CGameManager.DiseaseUnlockOrder[num - 1]);
  }

  public static bool AwardDiseaseCompletionAchievement(Disease.EDiseaseType type, bool mega = false)
  {
    if (!CGameManager.DiseaseCompletionAchievements.ContainsKey(type))
      return false;
    if (!mega)
      return CGameManager.AwardAchievement(CGameManager.DiseaseCompletionAchievements[type]);
    CGameManager.AwardAchievement(CGameManager.DiseaseCompletionAchievements[type]);
    return CGameManager.AwardAchievement(CGameManager.DiseaseCompletionMegaAchievements[type]);
  }

  public static bool AwardCureCompletionAchievement(Disease.ECureScenario type, bool mega = false)
  {
    if (!CGameManager.CureCompletionAchievements.ContainsKey(type))
      return false;
    if (!mega)
      return CGameManager.AwardAchievement(CGameManager.CureCompletionAchievements[type]);
    bool flag = CGameManager.AwardAchievement(CGameManager.CureCompletionAchievements[type]);
    return CGameManager.CureCompletionMegaAchievements.ContainsKey(type) ? CGameManager.AwardAchievement(CGameManager.CureCompletionMegaAchievements[type]) : flag;
  }

  public static bool AwardScenarioCompletionAchievement(string scenarioID, bool mega = false)
  {
    bool flag = CGameManager.ScenarioCompletionAchievements.ContainsKey(scenarioID);
    if (mega)
    {
      int num = CGameManager.ScenarioCompletionMegaAchievements.ContainsKey(scenarioID) ? 1 : 0;
      if (flag)
        CGameManager.AwardAchievement(new EAchievement?(CGameManager.ScenarioCompletionAchievements[scenarioID]));
      if (num != 0)
        return CGameManager.AwardAchievement(new EAchievement?(CGameManager.ScenarioCompletionMegaAchievements[scenarioID]));
    }
    else if (flag)
      return CGameManager.AwardAchievement(new EAchievement?(CGameManager.ScenarioCompletionAchievements[scenarioID]));
    return false;
  }

  public static string GetWorldDefconText()
  {
    Disease disease = CGameManager.localPlayerInfo.disease;
    if (disease.diseaseNoticed)
    {
      float num1 = 1f * (float) disease.totalDead / (float) World.instance.totalPopulation;
      float num2 = 1f * (float) disease.totalInfected / (float) World.instance.totalPopulation;
      if ((double) num1 > 0.20000000298023224)
        return CLocalisationManager.GetText("Humanity close to extinction");
      if ((double) num1 > 0.0099999997764825821)
        return CLocalisationManager.GetText("Plague threatens the world");
      if ((double) num2 > 0.5)
        return CLocalisationManager.GetText("%@ is global").Replace("%@", disease.name);
      return (double) num2 > 0.20000000298023224 ? CLocalisationManager.GetText("%@ is spreading fast").Replace("%@", disease.name) : CLocalisationManager.GetText("%@ has been spotted").Replace("%@", disease.name);
    }
    return disease.isCure ? CLocalisationManager.GetText("No new disease discovered") : CLocalisationManager.GetText("%@ has not been noticed").Replace("%@", disease.name);
  }

  public static string GetDefconText(
    bool zdayDone,
    float zombiePercent,
    float publicOrder,
    bool isDestroyed)
  {
    if (isDestroyed)
      return CLocalisationManager.GetText("No longer exists");
    if ((double) publicOrder <= 0.0)
      return CLocalisationManager.GetText("Total anarchy");
    if ((double) publicOrder < 0.30000001192092896)
      return CLocalisationManager.GetText("Close to anarchy");
    if ((double) publicOrder < 0.60000002384185791)
      return CLocalisationManager.GetText("Widespread disorder");
    return (double) publicOrder < 0.89999997615814209 || zdayDone && (double) zombiePercent > 0.0 ? CLocalisationManager.GetText("General disruption") : CLocalisationManager.GetText("Business as usual");
  }

  public static string GetDifficultyIcon(int diff)
  {
    return CGameManager.DifficultyIcons.ContainsKey((uint) diff) ? CGameManager.DifficultyIcons[(uint) diff] : "Rating_Empty";
  }

  public static string GetDiseaseTechBackground(Disease.EDiseaseType diseaseType, bool isResearched)
  {
    switch (diseaseType)
    {
      case Disease.EDiseaseType.NEURAX:
        return !isResearched ? "Hex_Unevolved_Nurax" : "Hex_Evolved_Nurax";
      case Disease.EDiseaseType.NECROA:
        return !isResearched ? "Hex_Unevolved_Necroa" : "Hex_Evolved_Necroa";
      case Disease.EDiseaseType.CURE:
        return !isResearched ? "Hex_Unevolved_Blue_Cure" : "Hex_Evolved_Blue_Cure";
      default:
        return !isResearched ? "Hex_Unevolved" : "Hex_Evolved";
    }
  }

  public static string GetTechBackground(Technology.EHexType techType, bool isResearched)
  {
    switch (techType)
    {
      case Technology.EHexType.GENERAL:
        return CGameManager.IsCureGame ? (!isResearched ? "Hex_Unevolved_Pink_Cure" : "Hex_Evolved_Pink_Cure") : (!isResearched ? "Hex_Unevolved" : "Hex_Evolved");
      case Technology.EHexType.NEURAX:
        return !isResearched ? "Hex_Unevolved_Nurax" : "Hex_Evolved_Nurax";
      case Technology.EHexType.NECROA:
        return CGameManager.IsCureGame ? (!isResearched ? "Hex_Unevolved_Red_Cure" : "Hex_Evolved_Red_Cure") : (!isResearched ? "Hex_Unevolved_Necroa" : "Hex_Evolved_Necroa");
      case Technology.EHexType.YELLOW:
        return CGameManager.IsCureGame ? (!isResearched ? "Hex_Unevolved_Yellow_Cure" : "Hex_Evolved_Yellow_Cure") : (!isResearched ? "Hex_Unevolved_Pota_Humans" : "Hex_Evolved_Pota_Humans");
      case Technology.EHexType.GREEN:
        return !isResearched ? "Hex_Unevolved_Pota_Apes" : "Hex_Evolved_Pota_Apes";
      case Technology.EHexType.VAMP_WHITE:
        return !isResearched ? "Hex_Unevolved_Vampire_White" : "Hex_Evolved_Vampire_White";
      case Technology.EHexType.VAMP:
        return !isResearched ? "Hex_Unevolved_Vampire" : "Hex_Evolved_Vampire";
      case Technology.EHexType.TILE_LIGHT_BLUE:
        return !isResearched ? "Hex_Unevolved_Cyan_Cure" : "Hex_Evolved_Cyan_Cure";
      case Technology.EHexType.TILE_BLUE:
        return !isResearched ? "Hex_Unevolved_Blue_Cure" : "Hex_Evolved_Blue_Cure";
      default:
        return !isResearched ? "Hex_Unevolved" : "Hex_Evolved";
    }
  }

  public static int GetSpeedrunScore(Disease.EDiseaseType disease, long score)
  {
    int[] numArray = new int[5]{ 865, 665, 515, 415, 365 };
    if (CGameManager.SpeedrunScoreThresholds.ContainsKey(disease))
      numArray = CGameManager.SpeedrunScoreThresholds[disease];
    for (int speedrunScore = 0; speedrunScore < numArray.Length; ++speedrunScore)
    {
      if (score > (long) numArray[speedrunScore])
        return speedrunScore;
    }
    return numArray.Length;
  }

  public static int GetDefconState(float priority)
  {
    for (int index = 0; index < CGameManager.DefconThresholds.Count; ++index)
    {
      if ((double) priority < (double) CGameManager.DefconThresholds[index])
        return index;
    }
    return 6;
  }

  public static Disease.EDiseaseType GetCureDiseaseType(Disease.ECureScenario scenario)
  {
    switch (scenario)
    {
      case Disease.ECureScenario.Cure_Bioweapon:
        return Disease.EDiseaseType.BIO_WEAPON;
      case Disease.ECureScenario.Cure_Fungus:
        return Disease.EDiseaseType.FUNGUS;
      case Disease.ECureScenario.Cure_Nanovirus:
        return Disease.EDiseaseType.NANO_VIRUS;
      case Disease.ECureScenario.Cure_Parasite:
        return Disease.EDiseaseType.PARASITE;
      case Disease.ECureScenario.Cure_Prion:
        return Disease.EDiseaseType.PRION;
      case Disease.ECureScenario.Cure_Virus:
        return Disease.EDiseaseType.VIRUS;
      case Disease.ECureScenario.Cure_DiseaseX:
        return Disease.EDiseaseType.DISEASEX;
      default:
        return Disease.EDiseaseType.BACTERIA;
    }
  }

  public static Disease.EDiseaseType GetCureDiseaseType(string scenario)
  {
    switch (scenario)
    {
      case "cure":
        return Disease.EDiseaseType.BACTERIA;
      case "cure_bioweapon":
        return Disease.EDiseaseType.BIO_WEAPON;
      case "cure_fungus":
        return Disease.EDiseaseType.FUNGUS;
      case "cure_nanovirus":
        return Disease.EDiseaseType.NANO_VIRUS;
      case "cure_parasite":
        return Disease.EDiseaseType.PARASITE;
      case "cure_prion":
        return Disease.EDiseaseType.PRION;
      case "cure_virus":
        return Disease.EDiseaseType.VIRUS;
      default:
        Debug.LogError((object) ("Unknown cure scenario '" + scenario + "' - defaulting to bacteria."));
        return Disease.EDiseaseType.BACTERIA;
    }
  }

  public static Disease.ECureScenario GetCureScenarioType(Disease.EDiseaseType disease)
  {
    switch (disease)
    {
      case Disease.EDiseaseType.VIRUS:
        return Disease.ECureScenario.Cure_Virus;
      case Disease.EDiseaseType.FUNGUS:
        return Disease.ECureScenario.Cure_Fungus;
      case Disease.EDiseaseType.PARASITE:
        return Disease.ECureScenario.Cure_Parasite;
      case Disease.EDiseaseType.PRION:
        return Disease.ECureScenario.Cure_Prion;
      case Disease.EDiseaseType.NANO_VIRUS:
        return Disease.ECureScenario.Cure_Nanovirus;
      case Disease.EDiseaseType.BIO_WEAPON:
        return Disease.ECureScenario.Cure_Bioweapon;
      case Disease.EDiseaseType.DISEASEX:
        return Disease.ECureScenario.Cure_DiseaseX;
      default:
        return Disease.ECureScenario.Cure_Standard;
    }
  }

  public static Disease.ECureScenario GetCureScenarioType(string scenario)
  {
    switch (scenario)
    {
      case "cure":
        return Disease.ECureScenario.Cure_Standard;
      case "cure_bioweapon":
        return Disease.ECureScenario.Cure_Bioweapon;
      case "cure_fungus":
        return Disease.ECureScenario.Cure_Fungus;
      case "cure_nanovirus":
        return Disease.ECureScenario.Cure_Nanovirus;
      case "cure_parasite":
        return Disease.ECureScenario.Cure_Parasite;
      case "cure_prion":
        return Disease.ECureScenario.Cure_Prion;
      case "cure_virus":
        return Disease.ECureScenario.Cure_Virus;
      default:
        Debug.LogError((object) ("Unknown cure scenario: " + scenario));
        return Disease.ECureScenario.None;
    }
  }

  public static ScenarioInformation GetCureScenario(Disease.EDiseaseType diseaseType)
  {
    switch (diseaseType)
    {
      case Disease.EDiseaseType.BACTERIA:
        return CGameManager.GetOfficialScenario("cure");
      case Disease.EDiseaseType.VIRUS:
        return CGameManager.GetOfficialScenario("cure_virus");
      case Disease.EDiseaseType.FUNGUS:
        return CGameManager.GetOfficialScenario("cure_fungus");
      case Disease.EDiseaseType.PARASITE:
        return CGameManager.GetOfficialScenario("cure_parasite");
      case Disease.EDiseaseType.PRION:
        return CGameManager.GetOfficialScenario("cure_prion");
      case Disease.EDiseaseType.NANO_VIRUS:
        return CGameManager.GetOfficialScenario("cure_nanovirus");
      case Disease.EDiseaseType.BIO_WEAPON:
        return CGameManager.GetOfficialScenario("cure_bioweapon");
      case Disease.EDiseaseType.CURETUTORIAL:
        return CGameManager.GetOfficialScenario("cure");
      default:
        Debug.LogError((object) ("Unknown disease type for cure mode: " + (object) diseaseType + " using standard bacteria scenario."));
        return CGameManager.GetOfficialScenario("cure");
    }
  }

  public static ScenarioInformation GetOfficialScenario(string scenarioInformationID)
  {
    ScenarioInformation officialScenario = CGameManager.OfficialScenarios.Find((Predicate<ScenarioInformation>) (a => a.id == scenarioInformationID));
    if (officialScenario != null)
      return officialScenario;
    Debug.LogError((object) ("No Official scenario found called '" + scenarioInformationID + "'"));
    return officialScenario;
  }

  private static void LoadOfficialScenarios()
  {
    CGameManager.OfficialScenarios.Clear();
    foreach (string directory in EncodedResources.instance.GetDirectories("Data/OfficialScenarios"))
    {
      TextAsset textAsset = EncodedResources.Load("Data/OfficialScenarios/" + (directory + "/scenario.txt"));
      if (!(bool) (UnityEngine.Object) textAsset)
      {
        Debug.Log((object) "scenario.txt not found");
      }
      else
      {
        ScenarioInformation scenarioInformation = DataImporter.ImportScenarioInformation(textAsset.text);
        if (scenarioInformation != null)
        {
          scenarioInformation.filePath = directory;
          scenarioInformation.id = scenarioInformation.scenIdentifier;
          CGameManager.OfficialScenarios.Add(scenarioInformation);
        }
        else
          Debug.Log((object) ("Invalid scenario: " + directory));
      }
    }
  }

  public static void UnlockAllScenariosForPlayer()
  {
    IPlayerInfo localPlayerInfo = CNetworkManager.network.LocalPlayerInfo;
    foreach (ScenarioInformation officialScenario in CGameManager.OfficialScenarios)
    {
      if (!localPlayerInfo.GetScenarioUnlocked(officialScenario.id))
        localPlayerInfo.SetScenarioUnlocked(officialScenario.id);
    }
  }

  public static void CheckUnlocks()
  {
    bool flag1 = false;
    IPlayerInfo localPlayerInfo = CNetworkManager.network.LocalPlayerInfo;
    List<Disease.EDiseaseType> ediseaseTypeList = new List<Disease.EDiseaseType>((IEnumerable<Disease.EDiseaseType>) (Enum.GetValues(typeof (Disease.EDiseaseType)) as Disease.EDiseaseType[]));
    ediseaseTypeList.Remove(Disease.EDiseaseType.SIMIAN_FLU);
    bool flag2 = true;
    bool flag3 = true;
    bool flag4 = true;
    if (localPlayerInfo is CPlayerInfoSteam)
      Debug.Log((object) ("CHECKING UNLOCKS FOR STEAM PLAYER: " + (object) (localPlayerInfo as CPlayerInfoSteam).steamID));
    else
      Debug.Log((object) ("CHECKING UNLOCKS FOR NON STEAM PLAYER: " + (object) localPlayerInfo));
    for (int index = 0; index < ediseaseTypeList.Count; ++index)
    {
      Disease.EDiseaseType ediseaseType = ediseaseTypeList[index];
      switch (ediseaseType)
      {
        case Disease.EDiseaseType.TUTORIAL:
        case Disease.EDiseaseType.FAKE_NEWS:
        case Disease.EDiseaseType.CURE:
        case Disease.EDiseaseType.CURETUTORIAL:
        case Disease.EDiseaseType.DISEASEX:
          continue;
        default:
          int diseaseCompletion = localPlayerInfo.GetDiseaseCompletion(ediseaseType);
          if (diseaseCompletion >= 3 && CGameManager.AwardDiseaseCompletionAchievement(ediseaseType, true))
            flag1 = true;
          if (diseaseCompletion >= 1)
          {
            if (CGameManager.AwardDiseaseCompletionAchievement(ediseaseType))
              flag1 = true;
            Disease.EDiseaseType? diseaseNext = CGameManager.GetDiseaseNext(ediseaseType);
            Debug.Log((object) ("DiseaseType: " + (object) ediseaseType + " val: " + (object) diseaseCompletion + " next: " + (object) diseaseNext));
            if (diseaseNext.HasValue && !localPlayerInfo.GetDiseaseUnlocked(diseaseNext.Value))
            {
              flag1 = true;
              localPlayerInfo.SetDiseaseUnlocked(diseaseNext.Value);
            }
          }
          if (diseaseCompletion >= 0 && !localPlayerInfo.GetDiseaseUnlocked(ediseaseType))
          {
            flag1 = true;
            localPlayerInfo.SetDiseaseUnlocked(ediseaseType);
          }
          if (diseaseCompletion < 3)
          {
            if (flag4)
              Debug.Log((object) ("DiseaseType: " + (object) ediseaseType + " val: " + (object) diseaseCompletion + " -> no mega achievement"));
            flag4 = false;
          }
          if (ediseaseType != Disease.EDiseaseType.SIMIAN_FLU)
          {
            if (diseaseCompletion < 1)
            {
              if (flag2)
                Debug.Log((object) ("DiseaseType: " + (object) ediseaseType + " val: " + (object) diseaseCompletion + " -> no normal cheats"));
              flag2 = false;
            }
            if (diseaseCompletion < 2)
            {
              if (flag3)
                Debug.Log((object) ("DiseaseType: " + (object) ediseaseType + " val: " + (object) diseaseCompletion + " -> no brutal cheats"));
              flag3 = false;
              continue;
            }
            continue;
          }
          continue;
      }
    }
    if (flag2)
    {
      CGameManager.AwardAchievement(new EAchievement?(EAchievement.A_UnlockCheats));
      localPlayerInfo.SetIntStat("UNLOCK_BRUTAL_CHEATS", 1);
    }
    if (flag3)
      localPlayerInfo.SetIntStat("UNLOCK_MEGA_CHEATS", 1);
    if (flag4)
      CGameManager.AwardAchievement(new EAchievement?(EAchievement.A_Disease_Master));
    bool flag5 = true;
    foreach (ScenarioInformation officialScenario in CGameManager.OfficialScenarios)
    {
      string id = officialScenario.id;
      if (id != null && !id.StartsWith("cure_") && !(id == "cure"))
      {
        int a = 0;
        for (int difficulty = 0; difficulty < 4; ++difficulty)
          a = Mathf.Max(a, localPlayerInfo.GetScenarioRating(officialScenario, difficulty));
        if (a >= 3)
        {
          bool mega = localPlayerInfo.CompletedEveryDiseaseInScenarioWith3Biohazards(officialScenario);
          CGameManager.AwardScenarioCompletionAchievement(officialScenario.id, mega);
        }
        else
        {
          if (flag5)
            Debug.Log((object) ("All scenarios achievement failed: mst complete " + officialScenario.id));
          flag5 = false;
        }
      }
    }
    if (flag5)
      CGameManager.AwardAchievement(new EAchievement?(EAchievement.A_Scenario_Master));
    bool flag6 = true;
    bool flag7 = true;
    bool flag8 = true;
    for (int index = 0; index < ediseaseTypeList.Count; ++index)
    {
      Disease.EDiseaseType disease = ediseaseTypeList[index];
      switch (disease)
      {
        case Disease.EDiseaseType.TUTORIAL:
        case Disease.EDiseaseType.FAKE_NEWS:
        case Disease.EDiseaseType.CURE:
        case Disease.EDiseaseType.CURETUTORIAL:
        case Disease.EDiseaseType.DISEASEX:
          continue;
        default:
          Disease.ECureScenario cureScenarioType = CGameManager.GetCureScenarioType(disease);
          if (cureScenarioType != Disease.ECureScenario.Cure_Standard || disease == Disease.EDiseaseType.BACTERIA)
          {
            Disease.ECureScenario? cureNext = CGameManager.GetCureNext(cureScenarioType);
            if (cureNext.HasValue)
            {
              localPlayerInfo.SetCureLocked(cureNext.Value);
              continue;
            }
            continue;
          }
          continue;
      }
    }
    for (int index = 0; index < ediseaseTypeList.Count; ++index)
    {
      Disease.EDiseaseType disease = ediseaseTypeList[index];
      switch (disease)
      {
        case Disease.EDiseaseType.TUTORIAL:
        case Disease.EDiseaseType.FAKE_NEWS:
        case Disease.EDiseaseType.CURE:
        case Disease.EDiseaseType.CURETUTORIAL:
        case Disease.EDiseaseType.DISEASEX:
          continue;
        default:
          Disease.ECureScenario cureScenarioType = CGameManager.GetCureScenarioType(disease);
          if (cureScenarioType != Disease.ECureScenario.Cure_Standard || disease == Disease.EDiseaseType.BACTERIA)
          {
            int cureCompletion = localPlayerInfo.GetCureCompletion(cureScenarioType);
            if (cureCompletion >= 1)
            {
              Disease.ECureScenario? cureNext = CGameManager.GetCureNext(cureScenarioType);
              Debug.Log((object) ("DiseaseType: " + (object) disease + " val: " + (object) cureCompletion + " next: " + (object) cureNext));
              if (cureNext.HasValue && !localPlayerInfo.GetCureUnlocked(cureNext.Value))
              {
                flag1 = true;
                localPlayerInfo.SetCureUnlocked(cureNext.Value);
              }
            }
            if (cureCompletion >= 0 && !localPlayerInfo.GetCureUnlocked(cureScenarioType))
            {
              flag1 = true;
              localPlayerInfo.SetCureUnlocked(cureScenarioType);
            }
            if (cureCompletion < 3)
            {
              if (flag8)
                Debug.Log((object) ("Cure DiseaseType: " + (object) disease + " val: " + (object) cureCompletion + " -> no mega achievement"));
              flag8 = false;
            }
            if (cureCompletion < 1)
            {
              if (flag6)
                Debug.Log((object) ("Cure DiseaseType: " + (object) disease + " val: " + (object) cureCompletion + " -> no normal cheats"));
              flag6 = false;
            }
            if (cureCompletion < 2)
            {
              if (flag7)
                Debug.Log((object) ("Cure DiseaseType: " + (object) disease + " val: " + (object) cureCompletion + " -> no brutal cheats"));
              flag7 = false;
              continue;
            }
            continue;
          }
          continue;
      }
    }
    if (flag6)
      localPlayerInfo.SetIntStat("UNLOCK_CURE_CHEATS", 1);
    if (flag7)
      localPlayerInfo.SetIntStat("UNLOCK_CURE_MEGA_CHEATS", 1);
    int num = flag8 ? 1 : 0;
    if (!flag1)
      return;
    CGameManager.CheckUnlockedAll();
    CNetworkManager.network.SaveLocalPlayerData();
  }

  public static void CheckUnlockedAll()
  {
    EAchievement[] values = Enum.GetValues(typeof (EAchievement)) as EAchievement[];
    for (int index = 0; index < values.Length; ++index)
    {
      if (values[index] != EAchievement.A_Fully_Evolved && values[index] != EAchievement.A_WormFoodTest && values[index] != EAchievement.A_CompleteNecroa && values[index] != EAchievement.A_CompleteNipahVirus && values[index] != EAchievement.A_CompleteFrozenVirus && values[index] != EAchievement.A_CompleteArtificialOrgans && values[index] != EAchievement.A_CompleteUnknownOrigin && values[index] != EAchievement.None && !CNetworkManager.network.LocalPlayerInfo.GetAchievement(values[index]))
        return;
    }
    CGameManager.AwardAchievement(new EAchievement?(EAchievement.A_Fully_Evolved));
  }

  public static int GetActiveAbilityCost(EAbilityType type, Disease disease)
  {
    ActiveAbility ability = CGameManager.game.Abilities[type];
    return (int) disease.GetActiveAbilityCost(ability, type);
  }

  public static string GetLeaderboardName(
    IGame.GameType game,
    Disease.EDiseaseType disease,
    string scenarioID)
  {
    switch (game)
    {
      case IGame.GameType.Classic:
      case IGame.GameType.SpeedRun:
      case IGame.GameType.Tutorial:
      case IGame.GameType.Cure:
        return game.ToString().ToLower() + "_" + disease.ToString().ToUpper();
      case IGame.GameType.Official:
        return game.ToString().ToLower() + "_" + scenarioID.ToUpper();
      default:
        Debug.LogError((object) ("Tried to get leaderboard name for game type: " + (object) game));
        return (string) null;
    }
  }

  public static string GetCurrentLeaderboardName()
  {
    switch (CGameManager.gameType)
    {
      case IGame.GameType.Classic:
      case IGame.GameType.SpeedRun:
      case IGame.GameType.Tutorial:
        return CGameManager.localPlayerInfo == null || CGameManager.localPlayerInfo.disease == null ? (string) null : CGameManager.gameType.ToString().ToLower() + "_" + CGameManager.localPlayerInfo.disease.diseaseType.ToString().ToUpper();
      case IGame.GameType.Official:
        return CGameManager.game.CurrentLoadedScenario == null ? (string) null : CGameManager.gameType.ToString().ToLower() + "_" + CGameManager.game.CurrentLoadedScenario.scenarioInformation.id.ToUpper();
      default:
        return (string) null;
    }
  }

  public static byte[] GetDiseaseEventData(Disease.EDiseaseType disease)
  {
    string empty = string.Empty;
    string path = !CGameManager.EventsPaths.ContainsKey(disease) ? CGameManager.EventsPaths[Disease.EDiseaseType.BACTERIA] : CGameManager.EventsPaths[disease];
    switch (CGameManager.gameType)
    {
      case IGame.GameType.VersusMP:
        path += "_mp";
        break;
      case IGame.GameType.CoopMP:
        path += "_mp";
        break;
    }
    return CGameManager.LoadGameData(path);
  }

  public static byte[] LoadGameData(string path)
  {
    switch (CGameManager.gameType)
    {
      case IGame.GameType.VersusMP:
        path = "Multiplayer/" + path;
        break;
      case IGame.GameType.CoopMP:
        path = "Cooperative/" + path;
        break;
    }
    TextAsset textAsset = EncodedResources.Load("Data/" + path);
    if ((bool) (UnityEngine.Object) textAsset)
      return textAsset.bytes;
    Debug.LogError((object) ("Could not load: Resources/Data/" + path + " as text asset"));
    return (byte[]) null;
  }

  public static string LoadGameText(string path)
  {
    switch (CGameManager.gameType)
    {
      case IGame.GameType.VersusMP:
        path = "Multiplayer/" + path;
        break;
      case IGame.GameType.CoopMP:
        path = "Cooperative/" + path;
        break;
    }
    TextAsset textAsset = EncodedResources.Load("Data/" + path);
    if ((bool) (UnityEngine.Object) textAsset)
      return textAsset.text.Replace("\r\n", "\n");
    Debug.LogError((object) ("Could not load: Resources/Data/" + path + " as text asset"));
    return (string) null;
  }

  public static GameEvent.EEventDiseaseType GetDiseaseEventType(Disease.EDiseaseType diseaseType)
  {
    if (diseaseType == Disease.EDiseaseType.NECROA)
      return GameEvent.EEventDiseaseType.NECROA;
    return diseaseType == Disease.EDiseaseType.SIMIAN_FLU ? GameEvent.EEventDiseaseType.SIMIAN : GameEvent.EEventDiseaseType.STANDARD;
  }

  public static Disease ImportDisease(string diseaseText, bool loading, Scenario scenario)
  {
    if (diseaseText == null)
      Debug.Log((object) "disease text is null");
    Disease disease = DataImporter.ImportDisease(CGameManager.gameType, diseaseText, loading, scenario);
    CGameManager.ImportCustomIcons(disease, scenario);
    return disease;
  }

  public static void ImportDiseaseTech(
    Disease disease,
    string diseaseText,
    bool loading,
    Scenario scenario)
  {
    DataImporter.ImportDiseaseTech(disease, diseaseText, loading, scenario);
    CGameManager.ImportCustomIcons(disease, scenario);
  }

  private static void ImportCustomIcons(Disease disease, Scenario scenario)
  {
    if (scenario == null || scenario.customIcons == null)
      return;
    for (int index = 0; index < disease.technologies.Count; ++index)
    {
      Technology technology = disease.technologies[index];
      if (scenario.customIcons.ContainsKey(technology.graphicOverlay))
        technology.customOverlay = scenario.customIcons[technology.graphicOverlay];
      if (scenario.customIcons.ContainsKey(technology.baseGraphic))
        technology.customGraphic = scenario.customIcons[technology.baseGraphic];
    }
    foreach (KeyValuePair<EAbilityType, ActiveAbility> ability in CGameManager.game.Abilities)
    {
      if (ability.Value.aaGraphicPc != null)
        ability.Value.customGraphic = !scenario.customIcons.ContainsKey(ability.Value.aaGraphicPc) ? (Texture2D) null : scenario.customIcons[ability.Value.aaGraphicPc];
      else
        Debug.LogError((object) ("Active Ability: " + (object) ability.Key + " definition has no AA Graphic set for PC"));
    }
  }

  public static string GetDisplayDate()
  {
    return CGameManager.currentGameDate.Day.ToString() + "-" + CGameManager.currentGameDate.Month.ToString() + "-" + CGameManager.currentGameDate.Year.ToString();
  }

  public static string GetAchievementDescription(EAchievement ach)
  {
    CGameManager.CacheAchievements();
    return !CGameManager.achievementLookup.ContainsKey((int) ach) ? "" : CGameManager.achievementLookup[(int) ach].description;
  }

  public static string GetAchievementTitle(EAchievement ach)
  {
    CGameManager.CacheAchievements();
    return !CGameManager.achievementLookup.ContainsKey((int) ach) ? "" : CGameManager.achievementLookup[(int) ach].title;
  }

  public static bool IsAchievementActive(EAchievement ach)
  {
    CGameManager.CacheAchievements();
    return CGameManager.achievementLookup.ContainsKey((int) ach) && CGameManager.achievementLookup[(int) ach].valid;
  }

  private static void CacheAchievements()
  {
    if (CGameManager.achievementLookup != null)
      return;
    CGameManager.achievementLookup = (IDictionary<int, CGameManager.AchievementData>) new Dictionary<int, CGameManager.AchievementData>();
    foreach (CGameManager.AchievementData achievementData in CGameManager.achievementData)
      CGameManager.achievementLookup[(int) achievementData.achievement] = achievementData;
  }

  public static int GetScenarioConst(string scenarioID)
  {
    if (CGameManager.scenarioConstList != null)
    {
      if (CGameManager.scenarioConstList.ContainsKey(scenarioID))
        return CGameManager.scenarioConstList[scenarioID];
    }
    else
      Debug.LogError((object) "No Const List Got");
    return -1;
  }

  public static double GetMidDouble(double s, double e, double sp, double ep, double input)
  {
    return sp + (input - s) * (ep - sp) / (e - s);
  }

  public static double GetRating(int score, int cons)
  {
    if (score == 0 || cons <= 0)
      return 0.0;
    double input = (double) score;
    double num1 = (double) cons / 10.0;
    double num2 = score < 75000 ? (score < 55000 ? (score < 30000 ? CGameManager.GetMidDouble(0.0, 30000.0, num1 - 8.0, num1 - 5.0, input) : CGameManager.GetMidDouble(30000.0, 55000.0, num1 - 5.0, num1, input)) : CGameManager.GetMidDouble(55000.0, 75000.0, num1, num1 + 2.0, input)) : num1 + 2.0;
    return num2 <= 0.0 ? 0.0 : num2;
  }

  public static double GetRating(string scenario)
  {
    if (CGameManager.constedScenarioList == null)
    {
      Debug.Log((object) "No Consted scenario found, skip!");
      return -1.0;
    }
    return CGameManager.constedScenarioList.Contains(scenario) ? CGameManager.GetRating(CSLocalUGCHandler.GetScenarioHighScore(scenario), CGameManager.GetScenarioConst(scenario)) : -1.0;
  }

  public static string GetScenarioConstString(string scenarioID)
  {
    return ((double) CGameManager.GetScenarioConst(scenarioID) / 10.0).ToString("N1");
  }

  public static void GetRatingList()
  {
    if (CGameManager.constedScenarioList == null || CGameManager.constedScenarioList.Count == 0)
    {
      Debug.LogError((object) "No consted scenario found");
    }
    else
    {
      CGameManager.scenarioScores = new List<CScenarioScore>();
      CGameManager.scenarioScoreDict = new Dictionary<string, int>();
      if (CSavesSteam.ExistsCloudData("PIFSL.dat"))
      {
        string str1 = CSavesSteam.LoadCloudData("PIFSL.dat").Replace("\r\n", "\n").Replace("\n\r", "\n");
        char[] chArray = new char[1]{ '\n' };
        foreach (string str2 in str1.Split(chArray))
        {
          if (!string.IsNullOrEmpty(str2) && str2.Contains(","))
          {
            string key = str2.Split(',')[0];
            int num = int.Parse(str2.Split(',')[1]);
            if (CGameManager.constedScenarioList.Contains(key))
              CGameManager.scenarioScoreDict.Add(key, num);
          }
        }
      }
      foreach (string constedScenario in CGameManager.constedScenarioList)
      {
        CScenarioScore cscenarioScore = new CScenarioScore();
        cscenarioScore.scenarioID = constedScenario;
        int b = 0;
        if (CGameManager.scenarioScoreDict.ContainsKey(constedScenario))
          b = CGameManager.scenarioScoreDict[constedScenario];
        cscenarioScore.score = Mathf.Max(CSLocalUGCHandler.GetScenarioHighScoreInitial(constedScenario), b);
        cscenarioScore.cons = CGameManager.GetScenarioConst(constedScenario);
        if (CGameManager.scenarioScoreDict.ContainsKey(constedScenario))
          CGameManager.scenarioScoreDict[constedScenario] = cscenarioScore.score;
        else
          CGameManager.scenarioScoreDict.Add(constedScenario, cscenarioScore.score);
        cscenarioScore.rating = CGameManager.GetRating(constedScenario);
        CGameManager.scenarioScores.Add(cscenarioScore);
      }
      CGameManager.scenarioScores.Sort(new Comparison<CScenarioScore>(CScenarioScore.CompareRating));
      CGameManager.b30 = new List<CScenarioScore>();
      for (int index = 0; index < 30; ++index)
        CGameManager.b30.Add(CGameManager.scenarioScores[index]);
      CGameManager.SaveFederalScenarioCloudScores();
    }
  }

  public static double GetPotential()
  {
    CGameManager.GetRatingList();
    if (CGameManager.b30 == null || CGameManager.b30.Count < 30)
    {
      Debug.LogError((object) "B30 is not full, must be something wrong here");
      return -1.0;
    }
    double num = 0.0;
    for (int index = 0; index < 30; ++index)
      num += CGameManager.b30[index].rating;
    return num / 30.0;
  }

  public static void GenerateRatingList()
  {
    CGameManager.GetRatingList();
    if (CGameManager.b30 == null)
    {
      Debug.Log((object) "No B30, skip!");
    }
    else
    {
      string contents = "Your Potential: " + CGameManager.GetPotential().ToString("N2") + "\nYour B30 List: \n";
      for (int index = 0; index < CGameManager.b30.Count; ++index)
      {
        if (CGameManager.b30[index].cons <= 99)
          contents = contents + "Const: " + CGameManager.GetScenarioConstString(CGameManager.b30[index].scenarioID) + "\t\tScore: " + CGameManager.b30[index].score.ToString("N0") + "\tRating: " + CGameManager.b30[index].rating.ToString("N3") + "\t" + CGameManager.scenarioNameTitlePair[CGameManager.b30[index].scenarioID] + "\n";
        else
          contents = contents + "Const: " + CGameManager.GetScenarioConstString(CGameManager.b30[index].scenarioID) + "\tScore: " + CGameManager.b30[index].score.ToString("N0") + "\tRating: " + CGameManager.b30[index].rating.ToString("N3") + "\t" + CGameManager.scenarioNameTitlePair[CGameManager.b30[index].scenarioID] + "\n";
      }
      Debug.Log((object) "B30 list written to desktop");
      File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "/B30.txt", contents);
    }
  }

  public static string GetScenarioDifficultyNum(string scenarioID)
  {
    return CSLocalUGCHandler.GetScenarioLevelString(CGameManager.GetScenarioConst(scenarioID));
  }

  public static bool IsFederalScenario(string scenario)
  {
    return CGameManager.game.CurrentLoadedScenario != null && !string.IsNullOrEmpty(CGameManager.game.CurrentLoadedScenario.filename) && CGameManager.game.CurrentLoadedScenario.filename.Contains(scenario);
  }

  public static bool IsGiantScenario()
  {
    return CGameManager.IsFederalScenario("五象鸑鷟") || CGameManager.IsFederalScenario("赫拉巨蝎") || CGameManager.IsFederalScenario("深海克拉肯") || CGameManager.IsFederalScenario("远古巨齿鲨") || CGameManager.IsFederalScenario("塞贝克鳄") || CGameManager.IsFederalScenario("塞贝克鳄（荒寂）");
  }

  public static bool SaveFederalScenarioCloudScores()
  {
    if (CGameManager.constedScenarioList == null || CGameManager.constedScenarioList.Count < 1)
    {
      Debug.Log((object) "No const list found, to protect your scores, no save will be made");
      return false;
    }
    if (CGameManager.scenarioScores != null && CGameManager.scenarioScores.Count > 0)
    {
      string data = "";
      foreach (KeyValuePair<string, int> keyValuePair in CGameManager.scenarioScoreDict)
        data = data + keyValuePair.Key + "," + keyValuePair.Value.ToString() + "\n";
      CSavesSteam.SaveCloudData("PIFSL.dat", data);
      Debug.Log((object) ("Successfully saved " + CGameManager.scenarioScoreDict.Count.ToString("N0") + " scores to Steam cloud data"));
      return true;
    }
    Debug.Log((object) "Cannot save scores, because it's null");
    return false;
  }

  public static int GetScenarioLibraryScore()
  {
    double scenarioLibraryScore = 0.0;
    CGameManager.GetRatingList();
    if (CGameManager.scenarioScores == null || CGameManager.scenarioScores.Count < 1)
    {
      Debug.LogError((object) "Scenario Score is not full, must be something wrong here");
      return -1;
    }
    foreach (CScenarioScore scenarioScore in CGameManager.scenarioScores)
    {
      if (scenarioScore.cons >= 20)
        scenarioLibraryScore += (double) (scenarioScore.score * scenarioScore.cons * scenarioScore.cons) / 100000.0;
    }
    return (int) scenarioLibraryScore;
  }

  public static string GetScenarioDifficultyRaw(string scenario)
  {
    if (CGameManager.scenarioDifficultyRaw != null && CGameManager.scenarioDifficultyRaw.ContainsKey(scenario))
      return CGameManager.scenarioDifficultyRaw[scenario];
    return scenario.Contains("时生虫ReCRAFT") ? "BYD Error" : "FTR ?";
  }

  public static string GetScenarioPackName(string scenario)
  {
    return CGameManager.scenarioPack != null && CGameManager.scenarioPack.ContainsKey(scenario) ? CGameManager.scenarioPack[scenario] : "zzz_Default";
  }

  public static int GetScenarioDate(string scenario)
  {
    return CGameManager.scenarioDate != null && CGameManager.scenarioDate.ContainsKey(scenario) ? CGameManager.scenarioDate[scenario] : 0;
  }

  public static void UpdateScenarioInfo()
  {
    string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + Path.DirectorySeparatorChar.ToString() + "Ndemic Creations" + Path.DirectorySeparatorChar.ToString() + "Plague Inc. Evolved" + Path.DirectorySeparatorChar.ToString() + "scenarioList.json";
    if (!File.Exists(path))
      return;
    CGameManager.scenarioData = JSON.Parse(File.ReadAllText(path));
    CGameManager.constedScenarioList = new List<string>();
    CGameManager.scenarioConstList = new Dictionary<string, int>();
    CGameManager.scenarioNameTitlePair = new Dictionary<string, string>();
    CGameManager.federalScenarioNames = new List<string>();
    CGameManager.scenarioUnlockConditionOverride = new Dictionary<string, List<CGameManager.ScenarioUnlockCondition>>();
    CGameManager.scenarioName = new Dictionary<string, string>();
    CGameManager.federalScenarioAuthorList = new Dictionary<string, string>();
    CGameManager.scenarioUnlockPotential = new Dictionary<string, int>();
    CGameManager.scenarioDifficultyRaw = new Dictionary<string, string>();
    CGameManager.scenarioPack = new Dictionary<string, string>();
    CGameManager.scenarioPackDisplayName = new Dictionary<string, string>();
    CGameManager.scenarioDate = new Dictionary<string, int>();
    foreach (JSONNode jsonNode1 in CGameManager.scenarioData["Scenarios"].AsArray)
    {
      if (!string.IsNullOrEmpty((string) jsonNode1["fileName"]))
      {
        string key = "PIFSL " + (string) jsonNode1["fileName"];
        if (jsonNode1["preload"].AsBool)
          CGameManager.federalScenarioNames.Add((string) jsonNode1["fileName"]);
        CGameManager.constedScenarioList.Add(key);
        CGameManager.scenarioConstList.Add(key, jsonNode1["constant"].AsInt);
        CGameManager.scenarioNameTitlePair.Add(key, (string) jsonNode1["names"][0]["Value"]);
        CGameManager.scenarioName.Add(key, (string) jsonNode1["pinyin"]);
        string str1 = (string) jsonNode1["groupName"];
        if (!string.IsNullOrEmpty(str1))
          CGameManager.scenarioPack.Add(key, str1);
        string str2 = (string) jsonNode1["groupDisplayName"];
        if (!string.IsNullOrEmpty(str2))
          CGameManager.scenarioPackDisplayName.Add(key, str2);
        int num = 480 * (jsonNode1["releaseDate"]["year"].AsInt - 2000) + 35 * jsonNode1["releaseDate"]["month"].AsInt + jsonNode1["releaseDate"]["day"].AsInt;
        CGameManager.scenarioDate.Add(key, num);
        string str3 = (string) jsonNode1["authorDisplayed"];
        if (!string.IsNullOrEmpty(str3))
          CGameManager.federalScenarioAuthorList.Add(key, str3);
        int asInt1 = jsonNode1["unlockPotential"].AsInt;
        if (asInt1 > 0)
          CGameManager.scenarioUnlockPotential.Add(key, asInt1);
        string str4 = (string) jsonNode1["type"] + " " + (string) jsonNode1["difficulty"]["rating"] + (jsonNode1["difficulty"]["plus"].AsBool ? "+" : "");
        CGameManager.scenarioDifficultyRaw.Add(key, str4);
        if (jsonNode1["unlockCondition"].AsArray.Count > 0)
        {
          List<CGameManager.ScenarioUnlockCondition> scenarioUnlockConditionList = new List<CGameManager.ScenarioUnlockCondition>();
          foreach (JSONNode jsonNode2 in jsonNode1["unlockCondition"].AsArray)
          {
            string scenario = "PIFSL " + (string) CGameManager.scenarioData["Scenarios"][jsonNode2["id"].AsInt - 1]["fileName"];
            int asInt2 = jsonNode2["score"].AsInt;
            scenarioUnlockConditionList.Add(new CGameManager.ScenarioUnlockCondition(scenario, asInt2));
          }
          CGameManager.scenarioUnlockConditionOverride.Add(key, scenarioUnlockConditionList);
        }
      }
    }
    Debug.Log((object) "Successfully got Scenario List!");
  }

  public static string GetScenarioPackDisplayName(string scenario)
  {
    return CGameManager.scenarioPackDisplayName != null && CGameManager.scenarioPackDisplayName.ContainsKey(scenario) ? CGameManager.scenarioPackDisplayName[scenario] : "Single";
  }

  public static void CallExternalMethod(
    string methodName,
    World world,
    Disease disease,
    Country country,
    LocalDisease localDisease)
  {
    if (!((UnityEngine.Object) CGameManager.game != (UnityEngine.Object) null) || CGameManager.game.CurrentLoadedScenario == null || !CGameManager.game.CurrentLoadedScenario.functionAllowed || !CGameManager.game.CurrentLoadedScenario.externalMethods.Contains(methodName))
      return;
    System.Type scenarioExternalLibrary = CGameManager.game.CurrentLoadedScenario.scenarioExternalLibrary;
    MethodInfo method = scenarioExternalLibrary.GetMethod(methodName);
    if (method != (MethodInfo) null)
    {
      object instance = Activator.CreateInstance(scenarioExternalLibrary);
      method.Invoke(instance, new object[4]
      {
        (object) world,
        (object) disease,
        (object) country,
        (object) localDisease
      });
    }
    else
      Debug.Log((object) ("No such method found: " + methodName));
  }

  public static object CallExternalMethodWithReturnValue(
    string methodName,
    World world,
    Disease disease,
    Country country,
    LocalDisease localDisease)
  {
    if (!((UnityEngine.Object) CGameManager.game != (UnityEngine.Object) null) || CGameManager.game.CurrentLoadedScenario == null || !CGameManager.game.CurrentLoadedScenario.functionAllowed || !CGameManager.game.CurrentLoadedScenario.externalMethods.Contains(methodName))
      return (object) null;
    System.Type scenarioExternalLibrary = CGameManager.game.CurrentLoadedScenario.scenarioExternalLibrary;
    MethodInfo method = scenarioExternalLibrary.GetMethod(methodName);
    if (method != (MethodInfo) null)
    {
      object instance = Activator.CreateInstance(scenarioExternalLibrary);
      return method.Invoke(instance, new object[4]
      {
        (object) world,
        (object) disease,
        (object) country,
        (object) localDisease
      });
    }
    Debug.Log((object) ("No such method found: " + methodName));
    return (object) null;
  }

  public static bool CheckExternalMethodExist(string methodName)
  {
    return (UnityEngine.Object) CGameManager.game != (UnityEngine.Object) null && CGameManager.game.CurrentLoadedScenario != null && CGameManager.game.CurrentLoadedScenario.functionAllowed && CGameManager.game.CurrentLoadedScenario.externalMethods.Contains(methodName);
  }

  public static void CallSidebarAppend(
    string[] sidebarTitle,
    string[] sidebarValue,
    World world,
    Disease disease,
    Country country,
    LocalDisease localDisease)
  {
    string name = "SidebarAppend";
    if (!((UnityEngine.Object) CGameManager.game != (UnityEngine.Object) null) || CGameManager.game.CurrentLoadedScenario == null || !CGameManager.game.CurrentLoadedScenario.functionAllowed || !CGameManager.game.CurrentLoadedScenario.externalMethods.Contains(name))
      return;
    System.Type scenarioExternalLibrary = CGameManager.game.CurrentLoadedScenario.scenarioExternalLibrary;
    MethodInfo method = scenarioExternalLibrary.GetMethod(name);
    if (method != (MethodInfo) null)
    {
      object instance = Activator.CreateInstance(scenarioExternalLibrary);
      method.Invoke(instance, new object[6]
      {
        (object) sidebarTitle,
        (object) sidebarValue,
        (object) world,
        (object) disease,
        (object) country,
        (object) localDisease
      });
    }
    else
      Debug.Log((object) ("No such method found: " + name));
  }

  public static void CallSidebarAppendRef(
    ref string[] sidebarTitle,
    ref string[] sidebarValue,
    World world,
    Disease disease,
    Country country,
    LocalDisease localDisease)
  {
    string name = "SidebarAppend";
    if (!((UnityEngine.Object) CGameManager.game != (UnityEngine.Object) null) || CGameManager.game.CurrentLoadedScenario == null || !CGameManager.game.CurrentLoadedScenario.functionAllowed || !CGameManager.game.CurrentLoadedScenario.externalMethods.Contains(name))
      return;
    System.Type scenarioExternalLibrary = CGameManager.game.CurrentLoadedScenario.scenarioExternalLibrary;
    MethodInfo method = scenarioExternalLibrary.GetMethod(name);
    if (method != (MethodInfo) null)
    {
      object instance = Activator.CreateInstance(scenarioExternalLibrary);
      method.Invoke(instance, new object[6]
      {
        (object) sidebarTitle,
        (object) sidebarValue,
        (object) world,
        (object) disease,
        (object) country,
        (object) localDisease
      });
    }
    else
      Debug.Log((object) ("No such method found: " + name));
  }

  public static (string[], string[]) CallSidebarAppendDouble(
    string[] sidebarTitle,
    string[] sidebarValue,
    World world,
    Disease disease,
    Country country,
    LocalDisease localDisease)
  {
    string name = "SidebarAppend";
    if ((UnityEngine.Object) CGameManager.game != (UnityEngine.Object) null && CGameManager.game.CurrentLoadedScenario != null && CGameManager.game.CurrentLoadedScenario.functionAllowed && CGameManager.game.CurrentLoadedScenario.externalMethods.Contains(name))
    {
      System.Type scenarioExternalLibrary = CGameManager.game.CurrentLoadedScenario.scenarioExternalLibrary;
      MethodInfo method = scenarioExternalLibrary.GetMethod(name);
      if (method != (MethodInfo) null)
      {
        object instance = Activator.CreateInstance(scenarioExternalLibrary);
        return ((string[], string[])) method.Invoke(instance, new object[6]
        {
          (object) sidebarTitle,
          (object) sidebarValue,
          (object) world,
          (object) disease,
          (object) country,
          (object) localDisease
        });
      }
      Debug.Log((object) ("No such method found: " + name));
    }
    return (sidebarTitle, sidebarValue);
  }

  public delegate void OnGameSpeedChangeEvent();

  public struct AchievementData
  {
    public EAchievement achievement;
    public string title;
    public string description;
    public bool valid;

    public AchievementData(EAchievement a, string t, string d)
    {
      this.achievement = a;
      this.title = t;
      this.description = d;
      this.valid = true;
    }

    public AchievementData(EAchievement a, string t, string d, bool v)
    {
      this.achievement = a;
      this.title = t;
      this.description = d;
      this.valid = v;
    }
  }

  public struct ScenarioUnlockCondition
  {
    public string unlockScenario;
    public int unlockScore;

    public ScenarioUnlockCondition(string scenario, int score)
    {
      this.unlockScenario = scenario;
      this.unlockScore = score;
    }
  }

  public enum EBlockType
  {
    BLOCK,
    GUEST_ONLY,
    ALLOW,
  }
}
