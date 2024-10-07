using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MentalHealthcare.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly MentalHealthDbContext _dbContext;

        public AdminController(MentalHealthDbContext DbContext)
        {
            _dbContext = DbContext;
        }
        [HttpPost]
        [Route("")]
        public async Task<ActionResult<int>> AddArticle (Article article)
        {

         article.ArticleId = 0;
        _dbContext.Set<Article>().Add(article);
           await _dbContext.SaveChangesAsync();    
              
        return Ok(article.ArticleId);
        }



    }
}
