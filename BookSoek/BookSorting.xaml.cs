using System;
using System.Collections.Generic;
using System.Drawing;
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
using static System.Reflection.Metadata.BlobBuilder;

namespace BookSoek
{
    /// <summary>
    /// Interaction logic for BookSorting.xaml
    /// </summary>
    public partial class BookSorting : Page
    {
        public List<BookModel> UnorderedList = new List<BookModel>();
        public List<BookModel> OrderedList = new List<BookModel>();
        public List<BookModel> UserList = new List<BookModel>();

        public int order;

        public int score = 0;

        String blankSource = @"C:\\Users\\lab_services_student\\source\\repos\\PROG7312_st10085360_POE\\BookSoek\\Images\\Blank.png";
        String goldSource = @"C:\\Users\\lab_services_student\\source\\repos\\PROG7312_st10085360_POE\\BookSoek\\Images\\Gold.png";


        

        public BookSorting()
        {
            InitializeComponent();
            FillTiles();
        }
        private void FillTiles()
        {


            GenerateCallNumbers();
            SortCallNumbers();

            order = 0;
            for (int i = 0; i < 10; i++)
            {
                string number = $"Item{i + 1}";

                Button button = (Button)FindName(number);

                

                if (button != null)
                {
                    button.Content = UnorderedList[i].CallNumber;
                    // Create a reference to the lambda expression
                    RoutedEventHandler clickHandler = (sender, e) =>
                    {
                        order++;
                        ButtonClick(button, order);
                    };

                    // Add the event handler to the button
                    button.Click += clickHandler;

                    // Store the reference to the event handler in the Tag property
                    button.Tag = clickHandler;
                }
            }
        }

        private void RemoveButtonEvents(Button button)
        {
            // Check if the Tag property contains a reference to the event handler
            if (button.Tag is RoutedEventHandler clickHandler)
            {
                // Remove the event handler
                button.Click -= clickHandler;
            }
        }


        private void ButtonClick(Object sender, int order)
        {
            Button button = (Button)sender;
            button.IsEnabled = false;
            
            string choice = (string)button.Content;

            BookModel b = new() { CallNumber = choice };

            UserList.Add(b);


            string DisplayName = "Sort" + order;
            Label label = (Label)FindName(DisplayName);

            Score.Content = order;

            if (label != null)
            {
                label.Visibility = Visibility.Visible;
                label.Content = choice;
            }
        }

        private void Retry_Click(object sender, RoutedEventArgs e)
        {
            
            order = 0;
            UnorderedList.Clear();
            OrderedList.Clear();
            for (int i = 0; i < 10; i++)
            {
                string btn = $"Item{i}";
                Button button = (Button )FindName(btn);

                if (button != null)
                {
                    RemoveButtonEvents(button);
                }
            }

            FillTiles();      





            Sort1.Visibility = Visibility.Hidden;
            Sort2.Visibility = Visibility.Hidden;
            Sort3.Visibility = Visibility.Hidden;
            Sort4.Visibility = Visibility.Hidden;
            Sort5.Visibility = Visibility.Hidden;
            Sort6.Visibility = Visibility.Hidden;
            Sort7.Visibility = Visibility.Hidden;
            Sort8.Visibility = Visibility.Hidden;
            Sort9.Visibility = Visibility.Hidden;
            Sort10.Visibility = Visibility.Hidden;

            Item1.IsEnabled = true;
            Item2.IsEnabled = true;
            Item3.IsEnabled = true;
            Item4.IsEnabled = true;
            Item5.IsEnabled = true;
            Item6.IsEnabled = true;
            Item7.IsEnabled = true;
            Item8.IsEnabled = true;
            Item9.IsEnabled = true;
            Item10.IsEnabled = true;

        }




        private void GenerateCallNumbers()
        {
            string[] existingSurnames = { "Smith", "Johnson", "Williams", "Jones", "Brown", "Davis", "Miller", "Wilson", "Moore", "Taylor" };

            Random random = new Random();
            String callNumber;

            for (int i = 0; i < 10; i++)
            {
                int firstThreeDigits = random.Next(0, 1000);
                int twoDigits = random.Next(0, 100);
                string surname = existingSurnames[random.Next(existingSurnames.Length)];

                callNumber = $"{firstThreeDigits:D3}.{twoDigits:D2} {surname.Substring(0, Math.Min(3, surname.Length))}";

                BookModel b = new() { CallNumber = callNumber};
                UnorderedList.Add(b);
            }
        }


        //SORTING ALGORITHM FOR SORTING THE NEW LIST INTO AN ALPHABETICAL LIST
        private void SortCallNumbers()
        {
            OrderedList = UnorderedList
            .OrderBy(BookModel => ExtractSections(BookModel.CallNumber)).ToList();
        }

        //THE ABOVE CODE ADAPTED FROM:
        //AUTHOR: RAHMAN, A
        //WEB PAGE: USING LINQ
        //WEBSITE : ILOVEDOTNET.ORG
        //URL: https://ilovedotnet.org/blogs/using-linq-where-to-filter-data/
        //DATE: 2022





        //EXTRACT THE INDIVIDUALS PARTS OF THE CALL NUMBER SO THAT LINQ CAN BE USED TO SORT THE LIST
        private static (int Numeric, string Alphabetic) ExtractSections(string callNumber)
        {
            string[] sections = callNumber.Split(' ');
            string numeric = sections[0];
            string alphabetic = sections[1];
            int Numeric = int.Parse(numeric.Split('.')[0]);

            return (Numeric, alphabetic);
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            score = 0;

            for (int i = 0; i < 10; i++)
            {
                if (UserList[i].CallNumber != OrderedList[i].CallNumber)
                {
                    string lbl = $"Sort{i + 1}";
                    Label label = (Label)FindName(lbl);
                    SolidColorBrush brush = new SolidColorBrush(Colors.Red);

                    label.Foreground = brush;


                }
                else
                {
                    score++;
                }
            }
            CalculateScore();
            Score.Content = score;
        }

        private void CalculateScore()
        {
            BitmapImage blank = new BitmapImage( new Uri(blankSource));
            BitmapImage gold = new BitmapImage( new Uri(goldSource));

            if (score == 10)
            {
                star1.Source = gold;
                star2.Source = gold;
                star3.Source = gold;
            }
            else if (score >= 8 && score < 10)
            {
                star1.Source = gold;
                star2.Source = gold;
                star3.Source = blank;
            }
            else if (score >= 5 && score < 8)
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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
        }



        //THE ABOVE CODE ADAPTED FROM:
        //AUTHOR: CHAND, M
        //WEB PAGE: SUBSTRING IN C#
        //WEBSITE : C# CORNER
        //URL: https://www.c-sharpcorner.com/UploadFile/mahesh/substring-in-C-Sharp/
        //DATE: 24/04/2023


    }
}
