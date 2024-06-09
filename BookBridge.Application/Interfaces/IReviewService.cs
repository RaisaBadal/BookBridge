using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookBridge.Application.Models.Request;

namespace BookBridge.Application.Interfaces
{ 
    public interface IReviewService:ICrud<ReviewModel,long>
    {
    }
}
