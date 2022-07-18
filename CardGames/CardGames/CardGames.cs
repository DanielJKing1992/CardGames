using System;
using System.Collections.Generic;
using System.Linq;

namespace CardGames
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("Press enter to play, or 1 to test unused methods: ");
			if (Console.ReadLine () == "1")
			{
				//Testing methods that aren't used by two games:
				Console.WriteLine ("The following is a test of methods not used in either of the two games.");
				Deck testDeck = new Deck ();

				Console.WriteLine ("Created new deck. Deck is empty: {0} (Should be true)", testDeck.IsEmpty ());
				foreach (Suit suit in Suit.VALUES)
				{
					foreach (Rank rank in Rank.VALUES)
						testDeck.AddCard (new Card (rank, suit));
				}


				CardCountHand hand1 = new CardCountHand ();

				Card card1 = testDeck.DealOne ();
				Card card2 = testDeck.DealOne ();
				Card card3 = testDeck.DealOne ();
				Console.WriteLine ("Added standard 52 cards to deck.");
				Console.WriteLine ("Dealt three cards. Cards remaining: {0} (Should be 49)", testDeck.GetCardsRemaining ());
				Console.WriteLine ("Deck Size: {0} (Should still be 52)", testDeck.GetDeckSize ());

				Console.WriteLine ("Deck is empty: {0} (Should be false)", testDeck.IsEmpty ());

				Console.WriteLine ("No cards added to hand yet. Hand is empty: {0} (Should be true)", hand1.IsEmpty ());
				hand1.AddCard (card1);
				hand1.AddCard (card2);

				Console.WriteLine ("Added two and three of spades to hand. Hand is empty: {0} (Should be false)", hand1.IsEmpty ());
				Console.WriteLine ("Hand contains {0}: {1} (Should be true)", card1, hand1.ContainsCard (card1));
				Console.WriteLine ("Hand contains {0}: {1} (Should be false)", card3, hand1.ContainsCard (card3));
				hand1.DiscardHand ();
				Console.WriteLine ("Discarded hand. Hand is empty: {0} (Should be true)", hand1.IsEmpty ());

				hand1.AddCard (card1);
				hand1.AddCard (card2);
				Console.WriteLine ("Added cards back into hand.");

				Console.WriteLine ("Position of {0}: {1} (Should be 0)", card1, hand1.FindCard (card1));
				Console.WriteLine ("Position of {0}: {1} (Should be -1 since not in hand)", card3, hand1.FindCard (card3));
				Console.WriteLine ("Card at index 0: {0} (Should be two of spades)", hand1.GetCardAtIndex (0));
				Console.WriteLine ("Removed {0}", hand1.RemoveCard (card1));
				Console.WriteLine ("Two of spades is in hand: {0} (Should be false)", hand1.ContainsCard (card1));

				hand1.DiscardHand ();
				hand1.AddCard (card1);
				hand1.AddCard (card2);
				Console.WriteLine ("Added cards back into hand.");

				Console.WriteLine ("Removed card at index 1, which was the {0} (Should be 3 of spades)", hand1.RemoveCard (1));
				Console.WriteLine ("Three of spades is in hand: {0} (Should be false)", hand1.ContainsCard (card2));
			}
			else
			{
				//Loop until player chooses not to start new game
				bool keepPlaying = true;
				do
				{
					//Ask user which game to play
					Console.Write ("Please enter 1 for CardCount, 2 for Blackjack: ");
					string userChoice = Console.ReadLine ();
					if (userChoice == "1")
					{
						//Create deck, fill with 52 cards, shuffle
						Deck cardCountDeck = new Deck ();
						//Creates one of each suit of each rank - 4 * 13 = 52 cards
						foreach (Suit suit in Suit.VALUES)
						{
							foreach (Rank rank in Rank.VALUES)
								cardCountDeck.AddCard (new Card (rank, suit));
						}

						cardCountDeck.Shuffle ();

						CardCountHand player1Hand = new CardCountHand ();
						CardCountHand player2Hand = new CardCountHand ();


						//Deal 8 cards, one by one, alternating between players
						for (int i = 0; i < 8; i++)
						{
							player1Hand.AddCard (cardCountDeck.DealOne ());
							player2Hand.AddCard (cardCountDeck.DealOne ());
						}

						//Score the hands, find winner
						int player1Score = player1Hand.EvaluateHand ();
						int player2Score = player2Hand.EvaluateHand ();
						int winner = player1Hand.CompareTo (player2Hand);
						string winnerText;

						//Set winnerText based on who won
						if (winner == 1)
							winnerText = "Player 1!";
						else if (winner == -1)
							winnerText = "Player 2!";
						else
							winnerText = "Tie Game!";
					
						//Print out each hand
						Console.Write (player1Hand);
						Console.WriteLine (" ---- Score: {0}", player1Score);
						Console.WriteLine ();

						Console.Write (player2Hand);
						Console.WriteLine (" ---- Score: {0}", player2Score);
						Console.WriteLine ();

						//Print out winner message
						Console.WriteLine ("Winner: {0}", winnerText);

					}
					else if (userChoice == "2")
					{
						Console.WriteLine ("One for bad, two for good.");

						//Create deck, add 52 cards six times, shuffle

						Deck blackJackDeck = new Deck ();
						for (int i = 0; i < 6; i++)
						{
							foreach (Suit suit in Suit.VALUES)
							{
								foreach (Rank rank in Rank.VALUES)
									blackJackDeck.AddCard (new Card (rank, suit));
							}
						}



						blackJackDeck.Shuffle ();

						BlackJackHand playerHand = new BlackJackHand ();
						BlackJackHand dealerHand = new BlackJackHand ();


						//Deal two cards each to player and dealer, one by one, alternating
						playerHand.AddCard (blackJackDeck.DealOne ());
						dealerHand.AddCard (blackJackDeck.DealOne ());
						playerHand.AddCard (blackJackDeck.DealOne ());
						dealerHand.AddCard (blackJackDeck.DealOne ());

						int playerScore = playerHand.EvaluateHand ();


						//Display both cards of player's hand, and it's score:
						Console.Write ("Player Hand: ");
						foreach (Card card in playerHand)
							Console.Write (card.GetRank ().GetSymbol () + card.GetSuit ().GetSymbol () + " ");
						Console.WriteLine (" ---- Score: {0}", playerScore);

						//Display dealer's top card only, and that card's score
						Console.WriteLine ("Dealer Hand: {0}{1}", dealerHand [1].GetRank ().GetSymbol (), dealerHand [1].GetSuit ().GetSymbol ());

						//Loop for player's turn - stop if player says so, or if player busts
						Console.Write ("Would you like to HIT (H) or STAND (S)? ");
						string hitOrStand = Console.ReadLine ();
						hitOrStand = hitOrStand.ToUpper ();

						bool playerBust = false;
						if (hitOrStand == "H")
						{
							do
							{
								Console.WriteLine ("Player hits.");
								playerHand.AddCard (blackJackDeck.DealOne ());
								playerScore = playerHand.EvaluateHand ();
								foreach (Card card in playerHand)
									Console.Write (card.GetRank ().GetSymbol () + card.GetSuit ().GetSymbol () + " ");
								Console.WriteLine (" ---- Score: {0}", playerScore);

								playerBust = (playerScore > 21);

								if (playerBust == false)
								{
									Console.Write ("Would you like to HIT (H) or STAND (S)? ");
									hitOrStand = Console.ReadLine ();
									hitOrStand = hitOrStand.ToUpper ();
								}
								else
									Console.WriteLine ("Uh-oh.");

							} while (hitOrStand == "H" && playerBust == false);
						}

						if (playerBust == true)
						{
							Console.WriteLine ("Player has gone over 21. Dealer wins.");
						}

						//Only need to do dealer's turn if player didn't bust
						else
						{
							//Write out player's final score, no need to show hand again
							Console.WriteLine ("Player stands at {0}", playerScore);
							int dealerScore = dealerHand.EvaluateHand ();

							//reveal dealer's hole card, show new score
							Console.Write ("Dealer Hand: ");
							foreach (Card card in dealerHand)
								Console.Write (card.GetRank ().GetSymbol () + card.GetSuit ().GetSymbol () + " ");
							Console.WriteLine (" ---- Score: {0}", dealerScore);

							//Dealer's turn: hit until over 17
							//Unnecessary to check for bust if already checking for over 17
							bool dealerBust = false;
							if (dealerScore < 17)
							{
								do
								{
									Console.WriteLine ("Dealer hits.");
									dealerHand.AddCard (blackJackDeck.DealOne ());
									dealerScore = dealerHand.EvaluateHand ();
									foreach (Card card in dealerHand)
										Console.Write (card.GetRank ().GetSymbol () + card.GetSuit ().GetSymbol () + " ");
									Console.WriteLine (" ---- Score: {0}", dealerScore);

									dealerBust = (dealerScore > 21);

									if (dealerBust == true)
										Console.WriteLine ("Uh-oh.");
								} while (dealerScore < 17);
							}

							if (dealerBust == true)
							{
								Console.WriteLine ("Dealer has gone over 21. Player wins.");
							}
							else
							{

								//Only need to evaluate and compare hands if neither party busted
								Console.WriteLine ("Dealer stands at {0}", dealerScore);
								Console.WriteLine ("---");
								Console.WriteLine ("---");
								Console.WriteLine ("---");

								//Calculate and display player and dealer scores
								playerScore = playerHand.EvaluateHand ();
								dealerScore = dealerHand.EvaluateHand ();
								Console.WriteLine ("Player Score: {0}", playerScore);
								Console.WriteLine ("Dealer Score: {0}", dealerScore);

								//Compare hands, print out winner as in CardCount
								int winner = playerHand.CompareTo (dealerHand);
								string winnerText;
								if (winner == 1)
									winnerText = "Player!";
								else if (winner == -1)
									winnerText = "Dealer!";
								else
									winnerText = "Tie Game!";
								Console.WriteLine ("Winner: {0}", winnerText);
							}
						}
					}

					//Ask if player wishes to start new game of either type
					Console.Write ("To play again, enter \"Y\": ");
					string playAgainChoice = Console.ReadLine ();
					keepPlaying = (playAgainChoice == "Y" || playAgainChoice == "y");

				} while (keepPlaying == true);
			}
		}

		public class Rank
		{
			//Rank has name and symbol, and is added into list
			private string name;
			public string Name
			{
				get
				{
					return name;
				}

				set
				{
					name = value;
				}
			}
			private string symbol;
			public string Symbol
			{
				get
				{
					return symbol;
				}
				set
				{
					symbol = value;
				}
			}

			public static List<Rank>VALUES = new List<Rank> (13);

			static Rank()
			{
				TWO = new Rank ("Two", "2");
             	THREE = new Rank("Three", "3");
             	FOUR = new Rank("Four", "4");
             	FIVE = new Rank("Five", "5");
             	SIX = new Rank("Six", "6");
             	SEVEN = new Rank("Seven", "7");
             	EIGHT = new Rank("Eight", "8");
	            NINE = new Rank("Nine", "9");
             	TEN = new Rank("Ten", "10");
             	JACK = new Rank("Jack", "J");
             	QUEEN = new Rank("Queen", "Q");
             	KING = new Rank("King", "K");
             	ACE = new Rank("Ace", "A");
			}

			public Rank(string name, string symbol)
			{
				Name = name;
				Symbol = symbol;
				VALUES.Add(this);
			}

			public string GetSymbol()
			{
				return symbol;
			}

			public string GetName()
			{
				return name;
			}

			public override string ToString()
			{
				return name;
			}

			public int CompareTo(Rank OtherRankObject)
			{
				//Compares rank - order is 2-->A
				int rank1Value = 0;
				int rank2Value = 0;
				for (int i = 0; i < Rank.VALUES.Count; i++)
				{
					if (Rank.VALUES [i].GetName () == this.GetName ())
						rank1Value = i;
					if (Rank.VALUES [i].GetName () == OtherRankObject.GetName ())
						rank2Value = i;
				}

				if (rank1Value < rank2Value)
					return -1;
				else if (rank1Value > rank2Value)
					return 1;
				else
					return 0;
			}

			public static Rank TWO;
			public static Rank THREE;
			public static Rank FOUR;
			public static Rank FIVE;
			public static Rank SIX;
			public static Rank SEVEN;
			public static Rank EIGHT;
			public static Rank NINE;
			public static Rank TEN;
			public static Rank JACK;
			public static Rank QUEEN;
			public static Rank KING;
			public static Rank ACE;
		}

		public class Suit
		{
			//Suit has name and symbol, and is added into list
			private string name;
			public string Name
			{
				get {
					return name;
				}
				set {
					name = value;
				}
			}
			private string symbol;
			public string Symbol
						{
				get {
					return symbol;
				}
				set {
					symbol = value;
				}
			}

			public static List<Suit> VALUES = new List<Suit> (4);

			static Suit()
			{
				SPADES = new Suit ("Spades", "\u2660");    
             	CLUBS = new Suit ("Clubs", "\u2663");      
             	DIAMONDS = new Suit ("Diamonds", "\u2666");
             	HEARTS = new Suit ("Hearts", "\u2665");    
			}

			public Suit(string name, string symbol)
			{
				Name = name;
				Symbol = symbol;
				VALUES.Add(this);
			}

			public string GetSymbol()
			{
				return symbol;
			}

			public string GetName()
			{
				return name;
			}

			public override string ToString()
			{
				return name;
			}

			public int CompareTo(Suit OtherSuitObject)
			{
				//Compares suit rank - order is spades, clubs, diamonds, hearts
				int suit1Value = 0;
				int suit2Value = 0;
				for (int i = 0; i < Suit.VALUES.Count; i++)
				{
					if (Suit.VALUES [i].GetName () == this.GetName ())
						suit1Value = i;
					if (Suit.VALUES [i].GetName () == OtherSuitObject.GetName ())
						suit2Value = i;
				}

				if (suit1Value < suit2Value)
					return -1;
				else if (suit1Value > suit2Value)
					return 1;
				else
					return 0;
			}

			public static Suit SPADES;
			public static Suit CLUBS;
			public static Suit DIAMONDS;
			public static Suit HEARTS;
		}

		public class Card
		{
			//Card has a rank and suit
			private Rank rank;
			public Rank Rank
			{
				get
				{
					return rank;
				}
				set
				{
					rank = value;
				}
			}
			private Suit suit;
			public Suit Suit
			{
				get
				{
					return suit;
				}
				set
				{
					suit = value;
				}
			}

			static Card()
			{
				
			}


			public Card(Rank rank, Suit suit)
			{
				Rank = rank;
				Suit = suit;
			}

			public Rank GetRank()
			{
				return rank;
			}

			public Suit GetSuit()
			{
				return suit;
			}

			public int CompareTo(Card OtherCardObject)
			{
				//Comparing cards returns result of comparing their ranks
				//Could extend by comparing suit for cards with the same rank
				Rank rank1 = this.GetRank ();
				Rank rank2 = OtherCardObject.GetRank ();

				int result = rank1.CompareTo (rank2);
				return result;
			}

			public override String ToString()
			{
				string cardInfo = this.GetRank() + " of " + this.GetSuit();
				return cardInfo;
			}
		}

		public class Deck : List<Card>
		{
			//Deck is a list of cards, adding cards is performed in main class
			static Deck()
			{
				
			}

			public Deck()
			{
				
			}

			public void AddCard (Card card)
			{
				this.Add (card);
			}

			public int CardsDealt = 0;

			public Card DealOne ()
			{
				Card cardToDeal = this [this.CardsDealt];
				CardsDealt++;
				return cardToDeal;
			}

			public int GetCardsRemaining ()
			{
				int cardsLeft = this.GetDeckSize () - CardsDealt;
				return cardsLeft;
			}

			public int GetDeckSize()
			{
				return this.Count;
			}

			public bool IsEmpty()
			{
				if (this.GetCardsRemaining() == 0)
					return true;
				else
					return false;
			}

			public void Shuffle ()
			{
				Random rnd = new Random ();

				Card card;

				int n = this.Count;
				//Shuffle only the cards remaining:

				//Picture deck as staying together, but computer knows what cards have been dealt off top.
				//Shuffle function only rearranges cards that haven't been dealt, and only puts them back
				//into places formerly occupied by un-dealt cards
				for (int currPos = this.CardsDealt; currPos < n; currPos++)
				{
					int r = currPos + (int)(rnd.NextDouble () * (n - currPos));
					card = this [r];
					this [r] = this [currPos];
					this [currPos] = card;
				}
			}

			public void RestoreDeck ()
			{
				//Since dealing doesn't change the deck (it just keeps track of how many cards are dealt),
				//restoring deck just requires moving back to zero.
				CardsDealt = 0;
			}
		}

		public abstract class Hand : List<Card>
		{
			//Hand is a list of cards, cards are added in main class by dealing from deck
			static Hand()
			{
				
			}

			public Hand()
			{
				
			}

			public void AddCard(Card card)
			{
				this.Add (card);
			}

			public abstract int CompareTo (Hand OtherHandObject);

			public bool ContainsCard(Card card)
			{
				bool inHand = false;
				foreach (Card handCard in this)
				{
					if (card.CompareTo (handCard) == 0)
						inHand = true;
				}
				return inHand;
			}

			public void DiscardHand()
			{
				//Removes cards from hand, but they won't return to deck until RestoreDeck is called
				this.Clear();
			}

			public int FindCard(Card card)
			{
				//Iterates thru hand list, returns index of first instance of given card
				//Returns -1 if not in hand
				int positionOfCard = -1;
				for (int i = 0; i < this.Count; i++)
				{
					if (this [i].CompareTo (card) == 0)
						positionOfCard = i;
				}
				return positionOfCard;
			}

			public Card GetCardAtIndex(int index)
			{
				//returns card at given index
				return this [index];
			}

			public int GetNumberOfCards()
			{
				return this.Count;
			}

			public bool IsEmpty()
			{
				return (this.Count == 0);
			}

			public Card RemoveCard(Card rmCard)
			{
				//Removes first instance of given card, unless not present
				//Returns the card that was removed - not sure why
				if (this.ContainsCard (rmCard))
				{
					this.Remove (rmCard);
				}
				else
				{
					Console.WriteLine ("Card is not in hand.");
				}
				return rmCard;
			}

			public Card RemoveCard(int index)
			{
				//Removes card at given index
				//Returns card that was removed - this one makes sense, since you might
				//not know which card you were removing
				Card cardToReturn = this [index];
				this.RemoveAt (index);
				return cardToReturn;

			}

			public abstract int EvaluateHand ();

			public override string ToString()
			{
				//Creates empty string, adds on each card's rank name and suit name
				string handDescription = "";
				foreach (Card card in this)
					handDescription = handDescription + card.GetRank().GetSymbol()+ card.GetSuit().GetSymbol() + " ";
				return handDescription;
			}
		}

		public class CardCountHand : Hand
		{
			//Extends hand, defines two abstract methods
			public override int EvaluateHand()
			{
				//Scores each card based on rank, adds score to running tally
				int handValue = 0;
				foreach (Card card in this)
				{
					if (card.GetRank ().ToString () == "Two")
						handValue += 2;
					else if (card.GetRank ().ToString () == "Three")
						handValue += 3;
					else if (card.GetRank ().ToString () == "Four")
						handValue += 4;
					else if (card.GetRank ().ToString () == "Five")
						handValue += 5;
					else if (card.GetRank ().ToString () == "Six")
						handValue += 6;
					else if (card.GetRank ().ToString () == "Seven")
						handValue += 7;
					else if (card.GetRank ().ToString () == "Eight")
						handValue += 8;
					else if (card.GetRank ().ToString () == "Nine")
						handValue += 9;
					else if (card.GetRank ().ToString () == "Ace")
						handValue += 1;
					else if (card.GetRank ().ToString () == "Ten" || card.GetRank ().ToString () == "Jack" || card.GetRank ().ToString () == "Queen" || card.GetRank ().ToString () == "King")
						handValue += 10;
				}
				return handValue;
			}

			public override int CompareTo(Hand OtherHandObject)
			{
				//Gets score of each hand, compares them- -1 for <, 0 for =, 1 for >
				int firstHandScore = this.EvaluateHand ();
				int secondHandScore = OtherHandObject.EvaluateHand ();

				if (firstHandScore > secondHandScore)
					return 1;
				else if (firstHandScore < secondHandScore)
					return -1;
				else
					return 0;
			}
		}

		public class BlackJackHand : Hand
		{
			//Another extension of hand
			public override int EvaluateHand()
			{
				//Same as above, except aces are worth 11 - which causes problems beyond scope of assignment
				int handValue = 0;
				foreach (Card card in this)
				{
					if (card.GetRank ().ToString () == "Two")
						handValue += 2;
					else if (card.GetRank ().ToString () == "Three")
						handValue += 3;
					else if (card.GetRank ().ToString () == "Four")
						handValue += 4;
					else if (card.GetRank ().ToString () == "Five")
						handValue += 5;
					else if (card.GetRank ().ToString () == "Six")
						handValue += 6;
					else if (card.GetRank ().ToString () == "Seven")
						handValue += 7;
					else if (card.GetRank ().ToString () == "Eight")
						handValue += 8;
					else if (card.GetRank ().ToString () == "Nine")
						handValue += 9;
					else if (card.GetRank ().ToString () == "Ace")
						handValue += 11;
					else if (card.GetRank ().ToString () == "Ten" || card.GetRank ().ToString () == "Jack" || card.GetRank ().ToString () == "Queen" || card.GetRank ().ToString () == "King")
						handValue += 10;
				}
				return handValue;
			}

			public override int CompareTo(Hand OtherHandObject)
			{
				//Same as in CardCountHand
				int firstHandScore = this.EvaluateHand ();
				int secondHandScore = OtherHandObject.EvaluateHand ();

				if (firstHandScore > secondHandScore)
					return 1;
				else if (firstHandScore < secondHandScore)
					return -1;
				else
					return 0;
			}
		}
	}
}