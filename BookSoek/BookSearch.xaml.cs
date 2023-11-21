using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.IO.Packaging;
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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Linq;

namespace BookSoek
{
    /// <summary>
    /// Interaction logic for BookSearch.xaml
    /// </summary>
    public partial class BookSearch : Page
    {
        private string filePath = "D:/UNI/SEMESTER2/PROG/POE/PROG7312_st10085360_POE_FINAL/callnumbers.txt";

        String blankSource = @"D:\UNI\SEMESTER2\PROG\POE\PROG7312_st10085360_POE_FINAL\BookSoek\Images\Blank.png";
        String goldSource = @"D:\UNI\SEMESTER2\PROG\POE\PROG7312_st10085360_POE_FINAL\BookSoek\Images\Gold.png";

        private List<TreeNode> nodes;

        private TreeNode RANDOM_TOP_LEVEL;
        private TreeNode RANDOM_MID_LEVEL;
        private TreeNode RANDOM_LOW_LEVEL;

        private List<TreeNode> top;
        private List<TreeNode> mid;
        private List<TreeNode> low;


        private TreeNode currentStepNode;

        private int penalty = 0;
        private int attempts = 0;
        private int score = 100;

        private int lvl = 1;

        private Stopwatch gameTimer = new Stopwatch();
        private TimeSpan timePenalty = TimeSpan.Zero;
        private DispatcherTimer uiTimer = new DispatcherTimer();

        public BookSearch()
        {
            InitializeComponent();
            
        }

        #region LOGIC FOR ESTABLISHING RANDOM GAME CALL NUMBER TO BE FOUND BY USER

        #region LOGIC FOR SEQUENTIALLY SELECTING A CALL NUMBER
        //THE FIRST NUMBER IS SELECTED FROM THE TOP LEVEL
        private TreeNode GetRandomTop(List<TreeNode> nodes)
        {
            Random random = new Random();

            top = GetTopLevelNodes(nodes);
            if (top.Count > 0)
            {
                int randomIndex = random.Next(top.Count);
                TreeNode randomNode = top[randomIndex];

                return randomNode;
            }

            return null;
            

        }
        //THE SECOND IS SELECTED BASED OFF THE TOP LEVEL CATEGORY
        private TreeNode GetRandomMid(TreeNode RANDOM)
        {
            Random random = new Random();

            mid = GetMiddleLevelNodes(nodes);

            List<TreeNode> rndMid = GetMiddleLevelNodes(RANDOM.Children);

            if (rndMid.Count > 0)
            {
                int randomIndex = random.Next(rndMid.Count);
                TreeNode randomNode = rndMid[randomIndex];

                return randomNode;
            }
            
            return null;
            

        }
        //THE THIRD IS SELECTED BASED OFF THE MID LEVEL CATEGORY
        private TreeNode GetRandomLow(TreeNode RANDOM)
        {
            Random random = new Random();

            low = GetLowestLevelNodes(nodes);
            List<TreeNode> rndLow = GetLowestLevelNodes(RANDOM.Children);

            if (rndLow.Count > 0)
            {
                int randomIndex = random.Next(rndLow.Count);
                TreeNode randomNode = rndLow[randomIndex];

                return randomNode;
            }
            return null;


        }
        #endregion

        #region LOGIC FOR RETRIEVING EACH INDIVIDUAL LEVEL FROM THE MAIN TREE AND CREATING INDIVIDUAL TREES
        //RETRIEVES ALL THE TOP LEVEL NODES
        //THIS METHOD IS RUN TO RETRIEVE THE CALL NUMBERS FROM THE RANDOM METHODS ABOVE
        private List<TreeNode> GetTopLevelNodes(List<TreeNode> nodes)
        {
            List<TreeNode> top = new List<TreeNode>();
            GetTopLevelNodesRecursive(nodes, top);
            return top;
        }
        //THIS METHOD IMMEDIATLY RETRIEVES THE FIRST NODES FROM THE TREE SINCE THEY ARE THE FIRST TO BE LOADED FROM THE TEXT FILE
        private void GetTopLevelNodesRecursive(List<TreeNode> nodes, List<TreeNode> currentLevelNodes)
        {
            foreach (TreeNode node in nodes)
            {
                    currentLevelNodes.Add(node);            
            }
        }

        //RETRIEVES ALL THE MIDDLE LEVEL NODES
        private List<TreeNode> GetMiddleLevelNodes(List<TreeNode> nodes)
        {
            List<TreeNode> middlelevelNodes = new List<TreeNode>();
            GetMiddleLevelNodesRecursive(nodes, middlelevelNodes);
            return middlelevelNodes;
        }
        //THIS METHOD INCORPERATES A LEVEL CHECK TO MAKE SURE THAT ONLY THE MIDDLE LEVEL NODES ARE RETRIEVED
        private void GetMiddleLevelNodesRecursive(List<TreeNode> nodes, List<TreeNode> currentLevelNodes)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Level == 2)
                {
                    currentLevelNodes.Add(node);
                    
                }
                else
                {
                    GetMiddleLevelNodesRecursive(node.Children, currentLevelNodes);
                }
                               
            }
            


        }

        //RETRIEVES ALL THE LOWEST LEVEL NODES
        private List<TreeNode> GetLowestLevelNodes(List<TreeNode> nodes)
        {
            List<TreeNode> lowestLevelNodes = new List<TreeNode>();
            GetLowestLevelNodesRecursive(nodes, lowestLevelNodes);
            return lowestLevelNodes;
        }
        //THIS METHOD ALSO INCORPERATES A LEVEL CHECK TO MAKE SURE THAT ONLY THE LOWEST LEVEL NODES ARE RETRIEVED
        private void GetLowestLevelNodesRecursive(List<TreeNode> nodes, List<TreeNode> lowestLevelNodes)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Level == 3)
                {
                    lowestLevelNodes.Add(node);
                }
                else 
                {
                    GetLowestLevelNodesRecursive(node.Children, lowestLevelNodes);
                }
            }
        }

        #endregion

        #endregion


        #region LOGIC FOR FILLING THE TREE VIEW WITH THE TREE DATA
        //FILLS THE TREE WITH CONTENT OF THE TEXTFILE
        private List<TreeNode> BuildTree(string path)
        {
            List<TreeNode> treeNodes = new List<TreeNode>();
            Stack<TreeNode> stack = new Stack<TreeNode>();

            foreach (String line in File.ReadLines(path)) 
            {
                //THE "|" SYMBOL IS REFERENCED AS THAT WAS USED TO IDENTIFY THE LEVEL OF THE CALL NUMBERS
                int level = GetLevel(line);
                string book = line.TrimStart('|', ' ');

                TreeNode node = new TreeNode(level, book);

                while (stack.Count > 0 && stack.Peek().Level >= level) 
                {
                    stack.Pop();
                }

                if (stack.Count > 0)
                {
                    stack.Peek().Children.Add(node);
                }
                else
                {
                    treeNodes.Add(node);
                }

                stack.Push(node);
            }

            return treeNodes;
        }

        //CONVERTS THE TREE STRUCTURE TO A TREE VIEW
        private TreeViewItem ConvertToTreeViewItem(TreeNode treeNode)
        {
            TreeViewItem treeViewItem = new TreeViewItem { Header = treeNode.Book };

            foreach (TreeNode childNode in treeNode.Children)
            {
                treeViewItem.Items.Add(ConvertToTreeViewItem(childNode));
            }

            return treeViewItem;
        }

        #endregion


        #region GAME LOGIC
        //RETURNS THE LEVEL OF THE CALL NUMBER
        private static int GetLevel(string line)
        {
            int level = 0;

            foreach (char c in line)
            {
                if (c == '|')
                {
                    level++;
                }
                else
                {
                    break;
                }
            }
            return level;
        }

        //ON CLICK EVENT FOR THE START BUTTON
        private void Start_Click(object sender, RoutedEventArgs e)
        {
            BookView.Items.Clear();
            StartGame();
        }

        //RESET METHOD 
        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            Start.IsEnabled = true;

            nodes.Clear();

            BookView.Items.Clear();

            score = 0;
            Score.Content = "";
            Time.Content = "";
            Time.Foreground = Brushes.White;

            attempts = 0;

            gameTimer.Reset();
            timePenalty = TimeSpan.Zero;

            ToFind.Content = "";


            lvl = 1;

            star1.Visibility = Visibility.Hidden;
            star2.Visibility = Visibility.Hidden;
            star3.Visibility = Visibility.Hidden;



        }

        //THIS METHOD HANDLES THE ON CLICK AND SELECTION OF STEPS IN THE TREE VIEW
        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            bool isCorrect = true;
            TreeViewItem selectedNode = (TreeViewItem)BookView.SelectedItem;
            
            
            //PROBLEMS HERE WITH NULL REFERENCE...
            
            


            if (selectedNode != null & lvl == 1)
            {
                    if (selectedNode.Header.ToString() == RANDOM_TOP_LEVEL.Book)
                    {
                        isCorrect = true;
                        lvl++;
                        selectedNode.Foreground = Brushes.Green;
                        selectedNode.IsSelected = false;
                        selectedNode.IsExpanded = true;
                    NextStep();
                        

                    }
                    else
                    {
                        isCorrect = false;
                        selectedNode.Foreground = Brushes.Red;
                        selectedNode.IsSelected = false;
                        selectedNode.IsExpanded = false;
                        selectedNode.IsEnabled = false;
                    }
                
            }
            else if (selectedNode != null & lvl == 2) {
                
                    if (selectedNode.Header.ToString() == RANDOM_MID_LEVEL.Book)
                    {
                        isCorrect = true;
                        lvl++;
                        selectedNode.Foreground = Brushes.Green;
                        selectedNode.IsSelected = false;
                        selectedNode.IsExpanded = true;
                    NextStep();

                }
                    else
                    {
                        isCorrect = false;
                        selectedNode.Foreground = Brushes.Red;
                        selectedNode.IsSelected = false;
                        selectedNode.IsExpanded = false;
                }
                
            }
            else if (selectedNode != null & lvl == 3) { 

                
                    if (selectedNode.Header.ToString() == RANDOM_LOW_LEVEL.Book)
                    {
                        isCorrect = true;
                        selectedNode.Foreground = Brushes.Green;
                        selectedNode.IsSelected = false;
                        selectedNode.IsExpanded = true;
                    NextStep();

                }
                    else
                    {
                        isCorrect = false;
                        selectedNode.Foreground = Brushes.Red;
                        selectedNode.IsSelected = false;
                        selectedNode.IsExpanded = false;
                }
                
            }

            if (!isCorrect)
            {
                attempts++;
            }




            if (selectedNode != null)
            {
                string selectedDescription = selectedNode.Header.ToString(); // Explicitly cast to string



                if (selectedDescription == RANDOM_LOW_LEVEL.Book)
                {
                    gameTimer.Stop();
                    CalculateScore(penalty);
                }
                else
                {
                    feedbackLabel.Content = "Wrong choice. Try again.";
                }
            }

            
        }

        //THIS METHOD STARTS THE GAME AND RUNS ALL NECASSARY METHODS
        private void StartGame()
        {
            Start.IsEnabled = false;

            nodes = BuildTree(filePath);

            RANDOM_TOP_LEVEL = GetRandomTop(nodes);
            RANDOM_MID_LEVEL = GetRandomMid(RANDOM_TOP_LEVEL);
            RANDOM_LOW_LEVEL = GetRandomLow(RANDOM_MID_LEVEL);

            String toSplit = RANDOM_LOW_LEVEL.Book.ToString();

            String toFind = toSplit.Remove(0, 3);

            ToFind.Content = toFind;

            currentStepNode = nodes.First(); // Start from the root node

            foreach (TreeNode rootNode in nodes)
            {
                TreeViewItem treeViewItem = ConvertToTreeViewItem(rootNode);
                BookView.Items.Add(treeViewItem);
            }



            gameTimer.Start();

            // Set up the UI timer
            uiTimer.Interval = TimeSpan.FromMicroseconds(1000);
            uiTimer.Tick += UiTimer_Tick;
            uiTimer.Start();



        }

        //THIS METHOD IS RUN EVERY SECOND, ALSO CHECKS IF THE USER IS TAKING TOO LONG ON A STEP
        private void UiTimer_Tick(object sender, EventArgs e)
        {
            TimeDisplay.Content = gameTimer.Elapsed.TotalSeconds.ToString("0.00");

            if (gameTimer.Elapsed.TotalSeconds > 10)
            {
                TimeDisplay.Foreground = Brushes.Red;
                
            }
        }

        //THIS METHOD RESETS THE TIMER WHEN THE USER SELECTS THE CORRECT NEXT STEP
        private void NextStep()
        {
            timePenalty += gameTimer.Elapsed;
            TimeDisplay.Foreground = Brushes.White;
            gameTimer.Restart();
        }


        //CALCULATES THE USERS SCORE BASED ON THEIR PERFORMANCE
        private void CalculateScore(int penalty)
        {
            if (timePenalty.TotalSeconds > 30)
            {
                Time.Foreground = Brushes.Red;

                if (timePenalty.TotalSeconds - 30 < 3)
                {
                    penalty += 3;
                }
                else if (timePenalty.TotalSeconds - 30 > 3 && timePenalty.TotalSeconds - 30 < 6)
                {
                    penalty += 6;
                }
                else if (timePenalty.TotalSeconds - 30 > 6 )
                {
                    penalty += 12;
                }
            }

            Time.Foreground = Brushes.Green;





            if (attempts > 0 && attempts < 5)
            {
                penalty += 2;
            }
            else if(attempts > 5 && attempts < 8)
            {
                penalty += 4;
            }
            else if(attempts > 8)
            {
                penalty += 10;
            }

            int finalPenalty = 0;

            if (penalty < 10)
            {
                finalPenalty = penalty * 2;
            }
            else if (penalty > 10 && penalty < 15)
            {
                finalPenalty = penalty * 4;
            }
            else if (penalty > 15)
            {
                finalPenalty = penalty * 6;
            }

            score = 100 - finalPenalty;


            star1.Visibility = Visibility.Visible;
            star2.Visibility = Visibility.Visible;
            star3.Visibility = Visibility.Visible;
            BitmapImage blank = new BitmapImage(new Uri(blankSource));
            BitmapImage gold = new BitmapImage(new Uri(goldSource));

            Score.Content = attempts + 3;
            Time.Content = timePenalty.TotalSeconds;



            if (score == 100)
            {
                star1.Source = gold;
                star2.Source = gold;
                star3.Source = gold;
            }
            else if (score >= 66 && score < 100)
            {
                star1.Source = gold;
                star2.Source = gold;
                star3.Source = blank;
            }
            else if (score >= 33 && score < 66)
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



        #endregion

        

    }
}
