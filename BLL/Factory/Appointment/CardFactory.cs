using BLL.Common;
using BLL.Interfaces;
using BLL.Interfaces.Appointment;
using DAL.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Factory.Appointment
{
    public class CardFactory : GenericFactory<IVMS_DBEntities, Card>
    {

    }
     public class CardFactorys : ICardFactory
     {
         private IGenericFactory<DAL.db.Card> _card;
         Result _result = new Result();
         private string tableName = "Card";

         public Result SaveCard(Card card)
         {
             Result _result = new Result();
             try
             {
                 _card = new CardFactory();
                 if (card.ID > 0)
                 {
                     _card.Edit(card);
                     _result = _card.Save();
                     if (_result.isSucess)
                     {
                         _result.message = _result.UpdateSuccessfull(tableName);
                     }                     
                 }
             }
             catch (Exception ex)
             {
                 _result.isSucess = false;
                 _result.message = ex.Message;
             }
             return _result;
         }
         public Card GetFreeCard(int deviceNo)
         {
             //int enrollmentID = 0;
             _card = new CardFactory();
             try
             {

                 var card = _card.FindBy(x => x.DeviceNO == deviceNo && x.CardNO == null).FirstOrDefault();
                 //if (data != null)
                 //{
                 //    enrollmentID = data.ID;
                 //}
                 return card;
             }
             catch (Exception e)
             {
                 throw e;
             }
         }

         public Result UnassignCard(int deviceNo, string cardNO)
         {
             _card = new CardFactory();
             try
             {

                 var card = _card.FindBy(x => x.DeviceNO == deviceNo && x.CardNO == cardNO).FirstOrDefault();
                 if (card != null)
                 {
                     card.CardNO = null;
                     _card.Edit(card);
                     _card.Save();
                     _result.isSucess = true; 
                 }                 
             }
             catch (Exception e)
             {
                 _result.isSucess =false; 
             }
             return _result;
         }

         public int GetAssignCardID(int deviceNo, string cardNO)
         {
             int cardID = 0;
             _card = new CardFactory();
             try
             {

                 var data = _card.FindBy(x => x.DeviceNO == deviceNo && x.CardNO == cardNO).FirstOrDefault();
                 if (data != null)
                 {
                     cardID = data.CardID;
                 }
                 return cardID;
             }
             catch (Exception e)
             {
                 throw e;
             }
         }
     }
}
