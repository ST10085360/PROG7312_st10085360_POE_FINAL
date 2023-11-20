using System;
using System.Collections.Generic;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace BookSoek
{
    /// <summary>
    /// Interaction logic for BookSearch.xaml
    /// </summary>
    public partial class BookSearch : Page
    {
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
        private int score = 0;

        public BookSearch()
        {
            InitializeComponent();

            string filePath = "D:/UNI/SEMESTER2/PROG/POE/PROG7312_st10085360_POE_FINAL/callnumbers.txt";

            nodes = BuildTree(filePath);

            RANDOM_TOP_LEVEL = GetRandomTop(nodes);
            RANDOM_MID_LEVEL = GetRandomMid(RANDOM_TOP_LEVEL);
            RANDOM_LOW_LEVEL = GetRandomLow(RANDOM_MID_LEVEL);

            MessageBox.Show($"{RANDOM_TOP_LEVEL.Book}, {RANDOM_MID_LEVEL.Book}, {RANDOM_LOW_LEVEL.Book}");
            currentStepNode = nodes.First(); // Start from the root node

            foreach (TreeNode rootNode in nodes)
            {
                TreeViewItem treeViewItem = ConvertToTreeViewItem(rootNode);
                BookView.Items.Add(treeViewItem);
            }
        }

        
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


        //RETRIEVES ALL THE TOP LEVEL NODES
        private List<TreeNode> GetTopLevelNodes(List<TreeNode> nodes)
        {
            List<TreeNode> top = new List<TreeNode>();
            GetTopLevelNodesRecursive(nodes, top);
            return top;
        }
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


        //FILLS THE TREE WITH CONTENT OF THE TEXTFILE
        private List<TreeNode> BuildTree(string path)
        {
            List<TreeNode> treeNodes = new List<TreeNode>();
            Stack<TreeNode> stack = new Stack<TreeNode>();

            foreach (String line in File.ReadLines(path)) 
            {
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

        private int lvl = 1;
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

                    }
                    else
                    {
                        isCorrect = false;
                        selectedNode.Foreground = Brushes.Red;
                        selectedNode.IsSelected = false;
                        selectedNode.IsExpanded = false;
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
                penalty++;
            }




            if (selectedNode != null)
            {
                string selectedDescription = selectedNode.Header.ToString(); // Explicitly cast to string



                if (selectedDescription == RANDOM_LOW_LEVEL.Book)
                {
                    MessageBox.Show($"Congratulations! You found the correct item in {penalty + 3} tries");

                }
                else
                {
                    feedbackLabel.Content = "Wrong choice. Try again.";
                }
            }

            CalculateScore(penalty);
        }


        //TO FINISH
        private void CalculateScore(int penalty)
        {
            
        }

        private void BookView_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
