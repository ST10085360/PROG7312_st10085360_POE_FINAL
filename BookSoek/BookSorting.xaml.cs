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

        public int order = 0;

        public int score = 0;

        public BookSorting()
        {
            InitializeComponent();
            GenerateCallNumbers();
            SortCallNumbers();
            FillTiles();
        }
        private void FillTiles()
        {
            for (int i = 0; i < 10; i++)
            {
                string number = $"Item{i + 1}";

                Button button = (Button)FindName(number);

                if (button != null)
                {
                    button.Content = UnorderedList[i].CallNumber;

                    button.Click += (sender, e) =>
                    {
                        ButtonClick(button, order);
                    };
                }
            }
        }

        private void ResetOptions()
        { 
            order = 0;
            InitializeComponent();
            GenerateCallNumbers();
            SortCallNumbers();
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
        private void ButtonClick(Button button, int i)
        {
            button.IsEnabled = false;
            order++;
            string choice = (string)button.Content;

            BookModel b = new() { CallNumber = choice };

            UserList.Add(b);


            string DisplayName = "Sort" + order;
            Label label = (Label)FindName(DisplayName);

            if (label != null)
            {
                label.Content = choice;
            }
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
                if (UserList[i].CallNumber == OrderedList[i].CallNumber)
                {
                    score++;
                }
            }
            Score.Content = score;
        }


        private void Retry_Click(object sender, RoutedEventArgs e)
        {
            ResetOptions();

        }





        //private void Item1_Click(object sender, RoutedEventArgs e)
        //{
        //    Item1.IsEnabled = false;
        //    order++;
        //    String book = Item1.Content.ToString();

        //    BookModel b = new() { CallNumber = book };
        //    UserList.Add(b);


        //    string DisplayName = $"Sort{order}";
        //    Label label = (Label)FindName(DisplayName);



        //}

        //private void Item2_Click(object sender, RoutedEventArgs e)
        //{
        //    Item2.IsEnabled = false;
        //    order++;
        //    String book = Item2.Content.ToString();

        //    BookModel b = new() { CallNumber = book };
        //    UserList.Add(b);



        //}

        //private void Item3_Click(object sender, RoutedEventArgs e)
        //{
        //    Item3.IsEnabled = false;
        //    order++;   
        //    String book = Item3.Content.ToString();

        //    BookModel b = new() { CallNumber = book };
        //    UserList.Add(b);    



        //}

        //private void Item4_Click(object sender, RoutedEventArgs e)
        //{
        //    Item4.IsEnabled = false;
        //    order++;   
        //    String book = Item4.Content.ToString();

        //    BookModel b = new() { CallNumber = book };
        //    UserList.Add(b);    



        //}

        //private void Item5_Click(object sender, RoutedEventArgs e)
        //{
        //    Item5.IsEnabled = false;
        //    order++;  
        //    String book = Item5.Content.ToString();

        //    BookModel b = new() { CallNumber = book };
        //    UserList.Add(b);   



        //}

        //private void Item6_Click(object sender, RoutedEventArgs e)
        //{
        //    Item6.IsEnabled = false;
        //    order++;   
        //    String book = Item6.Content.ToString();

        //    BookModel b = new() { CallNumber = book };
        //    UserList.Add(b);    



        //}

        //private void Item7_Click(object sender, RoutedEventArgs e)
        //{
        //    Item7.IsEnabled = false;
        //    order++;    
        //    String book = Item7.Content.ToString();

        //    BookModel b = new() { CallNumber = book };
        //    UserList.Add(b);   



        //}

        //private void Item8_Click(object sender, RoutedEventArgs e)
        //{
        //    Item8.IsEnabled = false;
        //    order++;   
        //    String book = Item8.Content.ToString();

        //    BookModel b = new() { CallNumber = book };
        //    UserList.Add(b);   




        //}

        //private void Item9_Click(object sender, RoutedEventArgs e)
        //{
        //    Item9.IsEnabled = false;
        //    order++;   
        //    String book = Item9.Content.ToString();

        //    BookModel b = new() { CallNumber = book };
        //    UserList.Add(b);   



        //}

        //private void Item10_Click(object sender, RoutedEventArgs e)
        //{
        //    Item10.IsEnabled = false;
        //    order++;   
        //    String book = Item10.Content.ToString();

        //    BookModel b = new() { CallNumber = book };
        //    UserList.Add(b);    



        //}

        //THE ABOVE CODE ADAPTED FROM:
        //AUTHOR: CHAND, M
        //WEB PAGE: SUBSTRING IN C#
        //WEBSITE : C# CORNER
        //URL: https://www.c-sharpcorner.com/UploadFile/mahesh/substring-in-C-Sharp/
        //DATE: 24/04/2023


    }
}
