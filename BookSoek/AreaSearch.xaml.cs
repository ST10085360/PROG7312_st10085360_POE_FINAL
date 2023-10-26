using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        #region VARIABLES

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

        //THIS DICTIONARY IS USED IN THE ACTUAL GAMEPLAY OF THE PROGRAM, IT GETS FILLED DYNAMICALLY BASED ON 
        //THE CURRENT GAMEMODE AND DIFFICULTY LEVEL
        private static Dictionary<string, string> inplay = new Dictionary<string, string>();

        public int score = 0;

        //SETS THE GAMEMODE:
        //EVEN NUMBER: MATCH CALL NUMBERS TO DESCRIPTIONS
        //ODD NUMBER: MATCH DESCRIPTIONS TO CALL NUMBER
        //THIS NUMBER WILL BE INCREMENTED AFTER EVERY GAME, ALLOWING FOR A FOOL PROOF SWITCH BETWEEN GAMEMODES
        private static int gamemode = 0;

        String blankSource = @"D:\UNI\SEMESTER2\PROG\POE\PROG7312_st10085360_POE_FINAL\BookSoek\Images\Blank.png";
        String goldSource = @"D:\UNI\SEMESTER2\PROG\POE\PROG7312_st10085360_POE_FINAL\BookSoek\Images\Gold.png";


        //THESE LISTS ARE USED TO GENERATE A MATCH THE COLUMN GAME BASED OFF OF RANDOMLY SELECTED 
        //CALL NUMBERS AND DESCRIPTIONS FROM THE LOWER AND GIHER LEVEL DICTIONARIES
        public List<String> GameNumbers = new List<String>();
        public List<BookModel> FullCallNumbers = new List<BookModel>();
        public List<MatchModel> Descriptions = new List<MatchModel>();
        public List<MatchModel> UserChoices = new List<MatchModel>();

        public SolidColorBrush brush;
        public String[] colours = { "Green", "Blue", "Red", "Pink" };



        //THESE 2 LISTS HANDLE THE CHOICES ON THE LEEFT THAT HAVE ALREADY BEEN CLICKED
        //THOSE BUTTONS ARE DISABLED SO THAT THE USER HAS TO PICK A NEW ONE
        public List<String> ChosenLeftOptions = new List<String>();
        public List<String> AvailableLeftOptions = new List<String>
        {
            "LEFT1",
            "LEFT2",
            "LEFT3",
            "LEFT4"
        };

        static Random random = new Random();

        public int index = 0;
        public String number;
        public String desc;

        //CLICK COUNT CHECKS IF THE USER HAS MADE THE MAXIMUM AMOUNT OF MATCHES
        //AND THEN DISABLES THE REMAINING BUTTONS
        public int clickCount = 1;

        #endregion

        public AreaSearch()
        {
            InitializeComponent();
            rbEasy.IsChecked = true;
            rbHard.IsChecked = false;
            SelectDifficulty();
            generateOptions();

        }



        #region LOGIC FOR ADDING ON-CLICK EVENTS TO THE BUTTONS

        //METHOD THAT FILLS ALL THE BUTTONS WITH CALL NUMBERS FROM THE GENERATED LIST
        //ALSO DYNAMICALLY ADDS ON CLICK EVENTS THAT HANDLE THE RE-ARRANGE FUNCTIONALITY
        private void FillTiles()
        {
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

        //EVENT HANDLER FOR THE BUTTONS ON THE LEFT
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

                number = GetNumber(button.Content.ToString());
                return;

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

                desc = button.Content.ToString();
                return;

            }

        }

        //EVENT HANDLER FOR THE BUTTONS ON THE RIGHT
        private void RightButtonClick(Object sender)
        {
            if (clickCount == 4)
            {
                RIGHT1.IsEnabled = false;
                RIGHT2.IsEnabled = false;
                RIGHT3.IsEnabled = false;
                RIGHT4.IsEnabled = false;
                RIGHT5.IsEnabled = false;
                RIGHT6.IsEnabled = false;
                RIGHT7.IsEnabled = false;
            }
            clickCount++;


            if (gamemode % 2 == 0)
            {
                Button button = (Button)sender;
                button.IsEnabled = false;
                button.Foreground = brush;
                desc = button.Content.ToString();

                UserChoices.Add(new MatchModel { key = number, description = desc });

                string lbl = $"match{index}";

                Label label = (Label)FindName(lbl);

                label.Content = number + " " + desc;
                index++;

                foreach (var item in AvailableLeftOptions)
                {
                    button = (Button)FindName(item);
                    button.IsEnabled = true;
                }

                return;

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
                index++;

                foreach (var item in AvailableLeftOptions)
                {
                    button = (Button)FindName(item);
                    button.IsEnabled = true;
                }

            }





        }

        #endregion



        #region LOGIC FOR FILLING LISTS WITH CALL NUMBERS AND DESCRIPTIONS

        //REFERENCE #2 WAS USED FOR FURTHER INSIGHT INTO HOW DICTIONARIES WORK

        //THIS METHOD FILLS THEE GAME AND DESCRIPTION LISTS WITH RANDOM CALL NUMBERS
        //AND THEIR RESPECTIVE DESCRIPTIONS, AS WELL AS AN EXTRA 3 OF EITHER
        //DEPENDING ON THE GAMEMODE
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
                return;

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
                return;


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

        //THIS METHOD EXTRACTS THE TOP LEVEL DIGITS OF THE CALL NUMBERS
        private string GetNumber(String callnumber)
        {
            string[] sections = callnumber.Split(' ');
            string numeric = sections[0];
            string Numeric = numeric.Split('.')[0];

            return (Numeric);
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

        #endregion



        #region LOGIC FOR SELECTING THE USERS FAVOURED DIFFICULTY

        //SWITCHES DIFFICULTY TO HARD 
        private void rbHard_Checked(object sender, RoutedEventArgs e)
        {
            rbEasy.IsChecked = false;

            inplay.Clear();
            SelectDifficulty();
            generateOptions();

            ResetGame();

        }

        //SWITCHES THE DIFFICULTY TO EASY
        private void rbEasy_Checked(object sender, RoutedEventArgs e)
        {
            rbHard.IsChecked = false;

            inplay.Clear();
            SelectDifficulty();
            generateOptions() ;

            ResetGame();

        }

        //THIS METHOD FILLS THE PLAYING DICTIONARY WITH THE HIGHER OR LOWER LEVEL CALL NUMBERS AND
        //DESCRIPTIONS BASED ON THE USERS CHOICE OF DIFFICULTY
        private void SelectDifficulty()
        {
            FullCallNumbers.Clear();
            GameNumbers.Clear();
            Descriptions.Clear();
            UserChoices.Clear();

            index = 0;

            if (rbEasy.IsChecked == true)
            {
                inplay.Clear();
                foreach (var item in higherLevel)
                {
                    inplay.Add(item.Key, item.Value);
                }
                return;
            }
            else if (rbHard.IsChecked == true)
            {
                inplay.Clear();
                foreach (var item in lowerLevel)
                {
                    inplay.Add(item.Key, item.Value);
                }
                return;
            }
        }

        #endregion



        #region ON-CLICK EVENTS THAT HANDLE THE FLOW OF THE GAMES

        //ALLOWS THE USER TO CONFIRM THEIR CHOICE OF MATCHES AND RETURNS THEIR SCORE
        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (clickCount == 5) 
            { 
                int level = 0;
                foreach (var item in UserChoices)
                {
                    string lbl = $"match{level}";
                    level++;
                    if (higherLevel.ContainsKey(item.key) && higherLevel[item.key] == item.description)
                    {
                        score++;
                    }
                    else
                    {
                        Label label = (Label)FindName(lbl);
                        HighlightIncorrectMatch(label);
                    }
                }

                CalculateScore();
                Score.Content = score;
                score = 0;
                rbEasy.IsEnabled = true;
                rbHard.IsEnabled = true;   
            }
            else
            {
            MessageBox.Show("Please complete all the matches.", "Wag n bietjie!", MessageBoxButton.OK, MessageBoxImage.Exclamation);

            }
        }

        //CALCULATES THE AMOUNT OF STARS THAT WILL BE AWARED TO THE USER
        private void CalculateScore()
        {
            star1.Visibility = Visibility.Visible;
            star2.Visibility = Visibility.Visible;
            star3.Visibility = Visibility.Visible;
            BitmapImage blank = new BitmapImage(new Uri(blankSource));
            BitmapImage gold = new BitmapImage(new Uri(goldSource));

            if (score == 4)
            {
                star1.Source = gold;
                star2.Source = gold;
                star3.Source = gold;
            }
            else if (score == 3)
            {
                star1.Source = gold;
                star2.Source = gold;
                star3.Source = blank;
            }
            else if (score >= 1 && score < 3)
            {
                star1.Source = gold;
                star2.Source = blank;
                star3.Source = blank;
            }
            else
            {
                star1.Source = blank;
                star2.Source = blank;
                star3.Source = blank;
            }
        }

        //ALLOWS THE USER TO RESET THEIR CHOICE OF MATCHES AND TRY AGAIN
        private void Retry_Click(object sender, RoutedEventArgs e)
        {
            ResetGame();
        }

        //RESETS THE CURRENT GAME
        private void ResetGame()
        {
            //RESET THE SELECTED BUTTONS FOR A NEW GAME
            ChosenLeftOptions.Clear();
            String[] range = { "LEFT1", "LEFT2", "LEFT3", "LEFT4" };
            AvailableLeftOptions.AddRange(range);


            clickCount = 1;    

            //RESET CURRENT BUTTON INDEX
            index = 0;

            //CLEAR USER SELECTED MATCHES
            UserChoices.Clear();

            ResetLeftButtons();
            ResetRightButtons();
            ResetMatchLabels();

            rbEasy.IsEnabled = true;
            rbHard.IsEnabled = true;

            star1.Visibility = Visibility.Hidden;
            star2.Visibility = Visibility.Hidden;
            star3.Visibility = Visibility.Hidden;

            Score.Content = "";

            FillTiles();
        }



        //ALLOWS THE USER TO START A BRAND NEW GAME. THE CHOSEN DIFFICULTY LEVEL WILL
        //REMAIN CONSISTENT UNTIL THE USER DECIDES TO CHANGE IT
        private void New_Click(object sender, RoutedEventArgs e)
        {
            //SWITCH TO NEW GAMEMODE
            gamemode++;

            //RESET THE SELECTED BUTTONS FOR A NEW GAME
            ChosenLeftOptions.Clear();
            String[] range = { "LEFT1", "LEFT2", "LEFT3", "LEFT4" };
            AvailableLeftOptions.AddRange(range);


            clickCount = 1;

            //RESET CURRENT BUTTON INDEX
            index = 0;

            //CLEAR USER SELECTED MATCHES
            UserChoices.Clear();

            FullCallNumbers.Clear();
            Descriptions.Clear();
            GameNumbers.Clear();

            ResetLeftButtons();
            ResetRightButtons();
            ResetMatchLabels();

            rbEasy.IsEnabled = true;
            rbHard.IsEnabled = true;

            star1.Visibility = Visibility.Hidden;
            star2.Visibility = Visibility.Hidden;
            star3.Visibility = Visibility.Hidden;

            Score.Content = "";

            generateOptions();
            FillTiles();
        }

        #endregion



        #region METHODS THAT HANDLE UI CHANGES

        //THESE METHODS ARE PURELY USED FOR CODE CLEANUP, 
        //UPDATING THE VISUALS OF LABELS OR BUTTONS IN BETWEEN
        //ALL THE METHODS AND IMPORTANT APPLICATION LOGIC MAKES 
        //IT MORE DIFFICULT AND TIME CONSUMING WHEN SEARCHING FOR BUGS


        private void ResetMatchLabels()
        {
            match0.Content = "";
            match1.Content = "";
            match2.Content = "";
            match3.Content = "";

            match0.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
            match1.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
            match2.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
            match3.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
        }

        private void HighlightIncorrectMatch(Label label)
        {
            label.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Red"));
        }
        
        private void ResetLeftButtons()
        {
            LEFT1.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF000000"));
            LEFT2.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF000000"));
            LEFT3.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF000000"));
            LEFT4.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF000000"));


            for (int i = 0; i < 4; i++)
            {
                string btn = $"LEFT{i + 1}";
                Button button = (Button)FindName(btn);


                if (button != null)
                {
                    button.IsEnabled = true;
                    RemoveButtonEvents(button);
                }
            }
        }
        
        private void ResetRightButtons()
        {

            RIGHT1.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF000000"));
            RIGHT2.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF000000"));
            RIGHT3.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF000000"));
            RIGHT4.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF000000"));
            RIGHT5.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF000000"));
            RIGHT6.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF000000"));
            RIGHT7.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF000000"));


            for (int i = 0; i < 7; i++)
            {
                string btn = $"RIGHT{i + 1}";
                Button button = (Button)FindName(btn);


                if (button != null)
                {
                    button.IsEnabled = true;
                    RemoveButtonEvents(button);
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

        #endregion

    }


    #region REFERENCES SECTION

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
         CORPERATE:      GeeksForGeeks
           WEBSITE:      geeksforgeeks
         PAGE NAME:      C# Dictionary with examples
     DATE ACCESSED:      26/10/2023
               URL:      https://www.geeksforgeeks.org/c-sharp-dictionary-with-examples/?ref=lbp#discuss
    
    */

    #endregion  
}
