using Application.DTOs;
using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{    
    public interface ICardService
    {
        /// <summary>
        /// Adds the card to the deck
        /// </summary>
        /// <param name="deck_id">The Id of the deck to which the card is added</param>
        /// <param name="front">The front side of the card</param>
        /// <param name="back">The back side of the card</param>
        /// <returns>True - if the card has been added successfully</returns>
        public bool AddCard(int deck_id, String front, String back);

        /// <summary>
        /// Updates the card
        /// </summary>
        /// <param name="card_id"></param>
        /// <param name="front">The front side of the card</param>
        /// <param name="back">The back side of the card</param>
        /// <returns>True - if the card has been updated successfully</returns>
        public bool UpdateCard(int card_id, String front, String back);

        /// <summary>
        /// Deletes the card
        /// </summary>
        /// <param name="id">The card Id</param>
        /// <returns>True - if the card has been deleted successfully</returns>
        public bool DeleteCard(int id);

        /// <summary>
        /// Selects the next card from the deck during training
        /// </summary>
        /// <param name="deck_id">The Id of the deck with which the user is training</param>
        /// <returns>The front & back sides of a card selected from the deck and the card type for training</returns>
        public CardDto GetNextCard(int deck_id);

        /// <summary>
        /// Increments the box index of the card
        /// </summary>
        /// <param name="id">The card Id</param>
        /// <returns>True - if the changes have been applied successfully</returns>
        public bool KnowledgeLevelIncrement(int id);

        /// <summary>
        /// Sets the box index of the card to 0 
        /// </summary>
        /// <param name="id">The card Id</param>
        /// <returns>True - if the changes have been applied successfully</returns>
        public bool KnowledgeLevelSetNull(int id);

        /// <summary>
        /// Checks if one user can view analytics of another user
        /// </summary>
        /// <param name="from_user">The user who is trying to view analytics</param>
        /// <param name="to_user">The user whose analytics the other user is trying to view</param>
        /// <returns>True - if from_user can view analytics of to_user</returns>
        public bool AuthorizeUserCard(string username, int card_id);
    }
}
