using MediatR;
using MentalHealthcare.Application.Common;
using MentalHealthcare.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Application.HomeScreenItems.Queries.GetAdvertisements
{
    public class GetAdvertisementsQuery : IRequest<PageResult<AdvertisementDto>>
    {
        public int IsActive { get; set; } = 3;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

    }
}
