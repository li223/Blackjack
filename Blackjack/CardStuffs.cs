using System.Collections.Generic;

namespace Blackjack
{
    public struct Card
    {
        public string Id { get; set; }
        public int Value { get; set; }
        public int SecondValue { get; set; }
    }

    public static class Deck
    {
        public static List<Card> GenerateCardDeck()
        {
            var suits = new[] { 'c', 'h', 's', 'd' };
            var templist = new List<Card>();
            int counter = 1;
            int suitcount = 0;
            for(var i = 0; i != 52; i++)
            {
                var idbase = $"c{counter}";
                if (counter < 10) idbase = $"c0{counter}";
                templist.Add(new Card()
                {
                    Id = $"{idbase}{suits[suitcount]}.png",
                    Value = (counter < 11) ? counter : 10,
                    SecondValue = (counter == 1) ? 11 : 0
                });

                if(counter == 13)
                {
                    counter = 1;
                    suitcount++;
                }
                else counter++;
            }
            return templist;
        }
    }
}
