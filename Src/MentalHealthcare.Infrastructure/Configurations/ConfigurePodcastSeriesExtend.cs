using MentalHealthcare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Infrastructure.Configurations
{
    public static class ConfigurePodcastSeriesExtend
    {


        public static void ConfigurePodcastSeries(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PodcastSeries>(entity =>
            {

                entity.HasKey(p => p.PodcastSeriesId); 

                entity.Property(p => p.PodcastSeriesId)
                    .UseIdentityColumn(10, 10);

                entity.Property(p => p.NameOfTopic)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasMany(p => p.Podcasts)
                 .WithOne(ps => ps.podcastSeries) 
                 .HasForeignKey(p => p.PodcastSeriesId)
                 .OnDelete(DeleteBehavior.Cascade);






            });
    }
    } }