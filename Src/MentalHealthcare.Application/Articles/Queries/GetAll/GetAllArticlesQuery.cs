﻿using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.Articles.Queries.GetAll
{
    public class GetAllArticlesQuery : IRequest<PageResult<ArticleDto>>
    { public string? SearchText { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10; }
}
