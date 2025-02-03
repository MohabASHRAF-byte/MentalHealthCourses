using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Domain.Repositories;

public interface IArticleRepository
{
    
        public Task<int> CreateArticleAsync(Article Article);
        public Task<Article> GetArticleByIdAsync(int Id);
        public Task<(int, IEnumerable<Article>)> GetAllArticlesAsync(string? search, int requestPageNumber, int requestPageSize);
        public Task DeleteArticleAsync(int ID_Article);
        public Task UpdateArticleAsync(Article articles);
        public Task DeleteArticlePhotosUrlsAsync(int articleId);




}

