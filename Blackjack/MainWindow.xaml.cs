using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Blackjack
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int CpuTotal = 0;
        public int PlayerTotal = 0;
        public Random Rand = new Random();
        public int NumberOfPlayers = 1;
        public List<Card> CardDeck { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            this.CardDeck = Deck.GenerateCardDeck();
        }
        
        private void Increment_Click(object sender, RoutedEventArgs e)
        {
            NumberOfPlayers++;
            this.NoPL.Content = NumberOfPlayers;
        }
        
        private void Decrement_Click(object sender, RoutedEventArgs e)
        {
            if (NumberOfPlayers > 1)
            {
                NumberOfPlayers--;
                this.NoPL.Content = NumberOfPlayers;
            }
        }
        
        private void Start_Click(object sender, RoutedEventArgs e)
        {
            //Cba to have more than PvC atm
            if(NumberOfPlayers > 1)
            {
                MessageBox.Show("Only player vs cpu is available at the moment, please select 1 player");
                return;
            }
            //Make sure we have a fresh deck
            this.CardDeck = Deck.GenerateCardDeck();
            this.TCtrl.SelectedIndex = 1;
            RunCard(PlayerType.User);
        }

        public void RunCard(PlayerType type)
        {
            var card = this.CardDeck[Rand.Next(0, this.CardDeck.Count)];
            this.CardDeck.Remove(card);
            this.CardImg.Source = new BitmapImage(new Uri($"{AppDomain.CurrentDomain.BaseDirectory}/Cards/{card.Id}"));
            if (type == PlayerType.User) this.PlayerTotal += ((card.SecondValue != 0) && ((this.PlayerTotal + 11) < 22)) ? card.SecondValue : card.Value;
            else this.CpuTotal += ((card.SecondValue != 0) && ((this.CpuTotal + 11) < 22)) ? card.SecondValue : card.Value;
            this.TotalBj.Content = $"Total: {PlayerTotal}";
        }

        private void Hit_Click(object sender, RoutedEventArgs e)
        {
            RunCard(PlayerType.User);
            if(this.PlayerTotal > 21)
            {
                this.TotalBj.Content = "Busted";
                this.Hit.IsEnabled = false;
                this.Stand.IsEnabled = false;
                CpuRun();
            }
            else if(this.PlayerTotal == 21)
            {
                this.TotalBj.Content = "21 Blackjack";
                CpuRun();
                this.Hit.IsEnabled = false;
                this.Stand.IsEnabled = false;
                CpuRun();
            }
        }

        private void Stand_Click(object sender, RoutedEventArgs e)
        {
            this.Hit.IsEnabled = false;
            this.Stand.IsEnabled = false;
            CpuRun();
        }

        public void CpuRun()
        {
            var maxnum = Rand.Next(14, 22);
            while (true)
            {
                RunCard(PlayerType.Cpu);

                if (this.CpuTotal >= maxnum)
                {
                    if (this.CpuTotal > 21)
                    {
                        if (this.PlayerTotal <= 21)
                        {
                            MessageBox.Show($"Player Wins:\nCPU:{this.CpuTotal}\nPlayer:{this.PlayerTotal}");
                            break;
                        }
                        else
                        {
                            MessageBox.Show($"Draw:\nCPU:{this.CpuTotal}\nPlayer:{this.PlayerTotal}");
                            break;
                        }
                    }
                    else if (this.CpuTotal == 21)
                    {
                        MessageBox.Show($"CPU Wins:\nCPU:{this.CpuTotal}\nPlayer:{this.PlayerTotal}");
                        break;
                    }
                    else if (this.CpuTotal > this.PlayerTotal)
                    {
                        if (this.CpuTotal <= 21)
                        {
                            MessageBox.Show($"CPU Wins:\nCPU:{this.CpuTotal}\nPlayer:{this.PlayerTotal}");
                            break;
                        }
                        else
                        {
                            MessageBox.Show($"Draw:\nCPU:{this.CpuTotal}\nPlayer:{this.PlayerTotal}");
                            break;
                        }
                    }
                    else
                    {
                        if (this.PlayerTotal <= 21)
                        {
                            MessageBox.Show($"Player Wins:\nCPU:{this.CpuTotal}\nPlayer:{this.PlayerTotal}");
                            break;
                        }
                        else if(this.CpuTotal <= 21)
                        {
                            MessageBox.Show($"CPU Wins:\nCPU:{this.CpuTotal}\nPlayer:{this.PlayerTotal}");
                            break;
                        }
                        else
                        {
                            MessageBox.Show($"Draw:\nCPU:{this.CpuTotal}\nPlayer:{this.PlayerTotal}");
                            break;
                        }
                    }
                }
            }
            Reset();
        }

        public void Reset()
        {
            this.CpuTotal = 0;
            this.PlayerTotal = 0;
            this.Hit.IsEnabled = true;
            this.Stand.IsEnabled = true;
            this.CardImg.Source = null;
            this.TotalBj.Content = "Total: 0";
            this.TCtrl.SelectedIndex = 0;
        }

        public enum PlayerType
        {
            User = 0,
            Cpu = 1
        }
    }
}
