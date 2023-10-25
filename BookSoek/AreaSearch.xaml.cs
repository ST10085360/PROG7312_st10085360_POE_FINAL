using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookSoek
{
    /// <summary>
    /// Interaction logic for AreaSearch.xaml
    /// </summary>
    /// 

    
    public partial class AreaSearch : Page
    {
        //DEFINED TO ALLOW USER TO PRACTISE MATCHING THE LOWER LEVEL CALL NUMBERS
        private static Dictionary<string, string> lowerLevel = new Dictionary<string, string>()
        {
            {"010", "Bibliographies"},
            {"020", "Library and information sciences"},
            {"030", "Encyclopaedias and books of facts"},
            {"040", "Unassigned (formerly Biographies)"},
            {"050", "Magazines, journals, and serials"},
            {"060", "Associations, organizations, and museums"},
            {"070", "News media, journalism, and publishing"},
            {"080", "Quotations"},
            {"090", "Manuscripts and rare books"},
            {"100", "Philosophy"},
            {"110", "Metaphysics"},
            {"120", "Epistemology"},
            {"130", "Parapsychology and occultism"},
            {"140", "Philosophical schools of thought"},
            {"150", "Psychology"},
            {"160", "Philosophical logic"},
            {"170", "Ethics"},
            {"180", "Ancient, medieval, and Eastern philosophy"},
            {"190", "Modern Western philosophy (19th-century, 20th-century)"},
            {"200", "Religion"},
            {"210", "Philosophy and theory of religion"},
            {"220", "The Bible"},
            {"230", "Christianity"},
            {"240", "Christian practice and observance"},
            {"250", "Christian orders and local church"},
            {"260", "Social and ecclesiastical theology"},
            {"270", "History of Christianity"},
            {"280", "Christian denominations"},
            {"290", "Other religions"},
            {"300", "Social sciences, sociology, and anthropology"},
            {"310", "Statistics"},
            {"320", "Political science"},
            {"330", "Economics"},
            {"340", "Law"},
            {"350", "Public administration and military science"},
            {"360", "Social problems and social services"},
            {"370", "Education"},
            {"380", "Commerce, communications, and transportation"},
            {"390", "Customs, etiquette, and folklore"},
            {"400", "Language"},
            {"410", "Linguistics"},
            {"420", "English and Old English languages"},
            {"430", "German and related languages"},
            {"440", "French and related languages"},
            {"450", "Italian, Romanian, and related languages"},
            {"460", "Spanish, Portuguese, Galician"},
            {"470", "Latin and Italic languages"},
            {"480", "Classical and modern Greek languages"},
            {"490", "Other languages"},
            {"500", "Science"},
            {"510", "Mathematics"},
            {"520", "Astronomy"},
            {"530", "Physics"},
            {"540", "Chemistry"},
            {"550", "Earth sciences and geology"},
            {"560", "Paleontology"},
            {"570", "Biology"},
            {"580", "Plants"},
            {"590", "Animals"},
            {"600", "Technology"},
            {"610", "Medicine and health"},
            {"620", "Engineering"},
            {"630", "Agriculture"},
            {"640", "Home and family management"},
            {"650", "Management and public relations"},
            {"660", "Chemical engineering"},
            {"670", "Manufacturing"},
            {"680", "Manufacture for specific uses"},
            {"690", "Construction of buildings"},
            {"700", "The Arts"},
            {"710", "Area planning and landscape architecture"},
            {"720", "Architecture"},
            {"730", "Sculpture, ceramics, and metalwork"},
            {"740", "Graphic arts and decorative arts"},
            {"750", "Painting"},
            {"760", "Printmaking and prints"},
            {"770", "Photography, computer art, film, video"},
            {"780", "Music"},
            {"790", "Recreational and performing arts"},
            {"800", "Literature"},
            {"810", "American literature in English"},
            {"820", "English and Old English literatures"},
            {"830", "German and related literatures"},
            {"840", "French and related literatures"},
            {"850", "Italian, Romanian, and related literatures"},
            {"860", "Spanish, Portuguese, Galician literatures"},
            {"870", "Latin and Italic literatures"},
            {"880", "Classical and modern Greek literatures"},
            {"890", "Other literatures"},
            {"900", "History"},
            {"910", "Geography and travel"},
            {"920", "Biography and genealogy"},
            {"930", "History of the ancient world (to c. 499)"},
            {"940", "History of Europe"},
            {"950", "History of Asia"},
            {"960", "History of Africa"},
            {"970", "History of North America"},
            {"980", "History of South America"},
            {"990", "History of other areas"}
        };

        //DEFINED TO ALLOW USER TO ONLY PRACTISE MATCHING THE HIGHEST LEVEL CALL NUMBERS
        private static Dictionary<string, string> higherLevel = new Dictionary<string, string>()
        {
            {"000", "General Knowledge" },
            {"100", "Philosophy"},
            {"200", "Religion"},
            {"300", "Social sciences, sociology, and anthropology"},
            {"400", "Language"},
            {"500", "Science"},
            {"600", "Technology"},
            {"700", "The Arts"},
            {"800", "Literature"},
            {"900", "History"}
        };

        private static Dictionary<string, string> inplay = new Dictionary<string, string>();

        public int score = 0;

        //SETS THE GAMEMODE:
        //EVEN NUMBER: MATCH CALL NUMBERS TO DESCRIPTIONS
        //ODD NUMBER: MATCH DESCRIPTIONS TO CALL NUMBER
        //THIS NUMBER WILL BE INCREMENTED AFTER EVERY GAME, ALLOWING FOR A FOOL PROOF SWITCH BETWEEN GAMEMODES
        int gamemode = 1;

        String blankSource = @"C:\\Users\\lab_services_student\\source\\repos\\PROG7312_st10085360_POE\\BookSoek\\Images\\Blank.png";
        String goldSource = @"C:\\Users\\lab_services_student\\source\\repos\\PROG7312_st10085360_POE\\BookSoek\\Images\\Gold.png";

        public List<String> GameNumbers = new List<String>();
        public List<BookModel> FullCallNumbers = new List<BookModel>();
        public List<MatchModel> Descriptions = new List<MatchModel>();
        public List<MatchModel> UserChoices = new List<MatchModel>();

        public SolidColorBrush brush;
        public String[] colours = { "Green", "Blue", "Red", "Pink" };

        public List<String> ChosenLeftOptions = new List<String>();
        public List<String> AvailableLeftOptions = new List<String>
        {
            "LEFT1",
            "LEFT2",
            "LEFT3",
            "LEFT4"
        };

        static Random random = new Random();

        public AreaSearch()
        {
            InitializeComponent();
            rbEasy.IsEnabled = true;
            rbEasy.IsChecked = false;
            rbHard.IsEnabled = true;
            rbHard.IsChecked = true;
            SelectDifficulty();
            FillTiles();

        }


        //METHOD THAT FILLS ALL THE BUTTONS WITH CALL NUMBERS FROM THE GENERATED LIST
        //ALSO DYNAMICALLY ADDS ON CLICK EVENTS THAT HANDLE THE RE-ARRANGE FUNCTIONALITY
        private void FillTiles()
        {
            generateOptions();

            if (gamemode % 2 == 0) 
            {
                for (int i = 0; i < 4; i++)
                {
                    string LEFT = $"LEFT{i + 1}";

                    Button button = (Button)FindName(LEFT);



                    if (button != null)
                    {
                        button.Content = FullCallNumbers[i].CallNumber;

                        RoutedEventHandler clickHandler = (sender, e) =>
                        {
                            LeftButtonClick(button);
                        };

                        button.Click += clickHandler;
                        button.Tag = clickHandler;
                    }
                }

                for (int i = 0; i < 7; i++)
                {
                    string RIGHT = $"RIGHT{i + 1}";

                    Button button = (Button)FindName(RIGHT);



                    if (button != null)
                    {
                        button.Content = Descriptions[i].description;

                        RoutedEventHandler clickHandler = (sender, e) =>
                        {
                            RightButtonClick(button);
                        };

                        button.Click += clickHandler;
                        button.Tag = clickHandler;
                    }
                }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    string LEFT = $"LEFT{i + 1}";

                    Button button = (Button)FindName(LEFT);



                    if (button != null)
                    {
                        button.Content = Descriptions[i].description;


                        RoutedEventHandler clickHandler = (sender, e) =>
                        {
                            LeftButtonClick(button);
                        };

                        button.Click += clickHandler;
                        button.Tag = clickHandler;
                    }
                }

                for (int i = 0; i < 7; i++)
                {
                    string RIGHT = $"RIGHT{i + 1}";

                    Button button = (Button)FindName(RIGHT);



                    if (button != null)
                    {
                        button.Content = FullCallNumbers[i].CallNumber;


                        RoutedEventHandler clickHandler = (sender, e) =>
                        {
                            RightButtonClick(button);
                        };

                        button.Click += clickHandler;
                        button.Tag = clickHandler;
                    }
                }

            }

            



        }

        //CHECKS THE BUTTON TAG FOR AN EXISTING EVENT
        private void RemoveButtonEvents(Button button)
        {
            if (button.Tag is RoutedEventHandler clickHandler)
            {
                button.Click -= clickHandler;
            }
        }

        public int index = 0;
        public String number;
        public String desc;
        private void LeftButtonClick(Object sender)
        {
            rbEasy.IsEnabled = false;
            rbHard.IsEnabled = false;

            if (gamemode % 2 == 0)
            {
                Button button = (Button)sender;
                button.IsEnabled = false;
                ChosenLeftOptions.Add(button.Name);
                AvailableLeftOptions.Remove(button.Name);

                LEFT1.IsEnabled = false; LEFT2.IsEnabled = false; LEFT3.IsEnabled = false; LEFT4.IsEnabled = false;


                string Colour = colours[index];
                brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Colour));

                button.Foreground = brush;

                index++;

                number = GetNumber(button.Content.ToString());
            }
            else
            {
                Button button = (Button)sender;
                button.IsEnabled = false;
                ChosenLeftOptions.Add(button.Name);
                AvailableLeftOptions.Remove(button.Name);

                LEFT1.IsEnabled = false; LEFT2.IsEnabled = false; LEFT3.IsEnabled = false; LEFT4.IsEnabled = false;


                string Colour = colours[index];
                brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Colour));

                button.Foreground = brush;

                index++;

                desc = button.Content.ToString();
            }

        }

        private void RightButtonClick(Object sender)
        {

            if (gamemode % 2 == 0)
            {
                Button button = (Button)sender;
                button.IsEnabled = false;
                button.Foreground = brush;
                desc = button.Content.ToString();

                UserChoices.Add(new MatchModel { key = number, description = desc });

                String lbl = "match" + index;

                Label label = (Label)FindName(lbl);

                label.Content = number + " " + desc; 

                foreach (var item in AvailableLeftOptions)
                {
                    button = (Button)FindName(item);
                    button.IsEnabled = true;
                }

            }
            else
            {
                Button button = (Button)sender;
                button.IsEnabled = false;
                button.Foreground = brush;
                number = GetNumber(button.Content.ToString());

                UserChoices.Add(new MatchModel { key = number, description = desc });

                String lbl = "match" + index;

                Label label = (Label)FindName(lbl);

                label.Content = number + " " + desc;

                foreach (var item in AvailableLeftOptions)
                {
                    button = (Button)FindName(item);
                    button.IsEnabled = true;
                }
            }       

        }


        private string GetNumber(String callnumber)
        {
            string[] sections = callnumber.Split(' ');
            string numeric = sections[0];
            string Numeric = numeric.Split('.')[0];

            return (Numeric);
        }


        private void generateOptions()
        {
            if (gamemode % 2 == 0)
            {

                List<string> CallNumbers = inplay.Keys.ToList();

                Shuffle(CallNumbers);

                GameNumbers = CallNumbers.Take(4).ToList();
                Descriptions = GameNumbers
                    .Select(key => new MatchModel
                    {
                        key = key,
                        description = inplay[key]
                    })
                    .ToList();

                for (int i = 0; i < 3; i++)
                {
                    string randomKey;
                    do
                    {
                        randomKey = CallNumbers[random.Next(CallNumbers.Count)];
                    } while (Descriptions.Any(match => match.key == randomKey));

                    Descriptions.Add(new MatchModel
                    {
                        key = randomKey,
                        description = inplay[randomKey]
                    });
                }

                //CALLING THIS METHOD TO TAKE THE GAME NUMBERS AND CREATE
                //PROPER CALL NUMBERS THAT WILL BE DISPLAYED TO THE USER
                GenerateCallNumbers();


                Shuffle(Descriptions);
            }
            else
            {
                List<string> CallNumbers = inplay.Keys.ToList();

                Shuffle(CallNumbers);

                GameNumbers = CallNumbers.Take(4).ToList();
                Descriptions = GameNumbers
                    .Select(key => new MatchModel
                    {
                        key = key,
                        description = inplay[key]
                    })
                    .ToList();

                for (int i = 0; i < 3; i++)
                {
                    string randomKey;
                    do
                    {
                        randomKey = CallNumbers[random.Next(CallNumbers.Count)];
                    } while (GameNumbers.Contains(randomKey));
                    GameNumbers.Add(randomKey);
                }

                //CALLING THIS METHOD TO TAKE THE GAME NUMBERS AND CREATE
                //PROPER CALL NUMBERS THAT WILL BE DISPLAYED TO THE USER
                GenerateCallNumbers();


                Shuffle(Descriptions);

            }
 
            

            


        }

        private void SelectDifficulty()
        {
            if (rbEasy.IsChecked == true)
            {
                inplay.Clear();
                foreach (var item in higherLevel)
                {
                    inplay.Add(item.Key, item.Value);
                }
            }
            else if (rbHard.IsChecked == true)
            {
                inplay.Clear();
                foreach (var item in lowerLevel)
                {
                    inplay.Add(item.Key, item.Value);
                }
            }
        }

        //METHOD THAT GENERATES RANDOM CALL NUMBERS
        private void GenerateCallNumbers()
        {
            string[] existingSurnames = { "Smith", "Johnson", "Williams", "Jones", "Brown", "Davis", "Miller", "Wilson", "Moore", "Taylor" };

            Random random = new Random();
            String callNumber;

            for (int i = 0; i < GameNumbers.Count; i++)
            {
                int firstThreeDigits = Int32.Parse(GameNumbers[i]);
                int twoDigits = random.Next(0, 100);
                string surname = existingSurnames[random.Next(existingSurnames.Length)];

                callNumber = $"{firstThreeDigits:D3}.{twoDigits:D2} {surname.Substring(0, Math.Min(3, surname.Length))}";

                BookModel b = new() { CallNumber = callNumber };
                FullCallNumbers.Add(b);
            }
        }


        //THIS IS THE FISHER _YATES ALGORITHM
        //IT WAS NAMED AFTER RONALD FISHER AND FRANK YATES
        //ADAPTED FROM: REFERENCE #1
        static void Shuffle<T>(IList<T> list)
        {
            int i = list.Count;
            while (i > 1)
            {
                i--;
                int j = random.Next(i + 1);
                T value = list[j];
                list[j] = list[i];
                list[i] = value;
            }
        }

    


    }

    /*
    ======================================
                 REFERENCES              
    ======================================
    
                    (1)
    --------------------------------------
            AUTHOR:      ADWESH
         CORPERATE:      MEDIUM
           WEBSITE:      MEDIUM
         PAGE NAME:      Shuffling algorithms and randomization to improve algorithm‘s runtime. 
    DATE PUBLISHED:      7/8/2018
     DATE ACCESSED:      25/10/2023
               URL:      https://medium.com/nerd-for-tech/shuffling-algorithms-and-randomization-to-improve-algorithm-s-runtime-47f7fc705df
    

                    (2)
    --------------------------------------
            AUTHOR:      
         CORPERATE:      
           WEBSITE:      
         PAGE NAME:      
    DATE PUBLISHED:      
     DATE ACCESSED:      
               URL:      
    

                    (3)
    --------------------------------------
            AUTHOR:      
         CORPERATE:      
           WEBSITE:      
         PAGE NAME:      
    DATE PUBLISHED:      
     DATE ACCESSED:      
               URL:      
    

                    (4)
    --------------------------------------
            AUTHOR:      
         CORPERATE:      
           WEBSITE:      
         PAGE NAME:      
    DATE PUBLISHED:      
     DATE ACCESSED:      
               URL:      
    

    
    



    */
}
