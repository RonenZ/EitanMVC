﻿using Eitan.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eitan.Data
{
    public class EitanDbContext : DbContext
    {
        public DbSet<Eitan.Models.Artist> Artists { get; set; }

        public DbSet<Eitan.Models.Song> Songs { get; set; }

        public DbSet<Eitan.Models.SEO> SEOs { get; set; }

        public DbSet<Eitan.Models.Genre> Genres { get; set; }

        public DbSet<Eitan.Models.Label> Labels { get; set; }

        public DbSet<Eitan.Models.News> News { get; set; }

        public DbSet<Eitan.Models.Project> Projects { get; set; }

        public DbSet<Eitan.Models.ProjectType> ProjectTypes { get; set; }

        public DbSet<Eitan.Models.Release> Releases { get; set; }

        public DbSet<Eitan.Models.Client> Clients{ get; set; }

        public DbSet<Eitan.Models.Page> Pages { get; set; }

        /// <summary>
        /// on Model Created.
        /// </summary>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Release>()
                .HasOptional(r => r.SEO)
                .WithMany()
                .HasForeignKey(r => r.SeoId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Page>()
            .HasOptional(r => r.SEO)
            .WithMany()
            .HasForeignKey(r => r.SeoId)
            .WillCascadeOnDelete(true);

            modelBuilder.Entity<Project>()
            .HasOptional(r => r.SEO)
            .WithMany()
            .HasForeignKey(r => r.SeoId)
            .WillCascadeOnDelete(true);

            modelBuilder.Entity<News>()
             .HasOptional(r => r.SEO)
             .WithMany()
             .HasForeignKey(r => r.SeoId)
             .WillCascadeOnDelete(true);

            modelBuilder.Entity<Project>()
                .HasRequired(p => p.Type)
                .WithMany()
                .HasForeignKey(p => p.TypeID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Release>()
                .HasRequired(r => r.Label)
                .WithMany()
                .HasForeignKey(r => r.LabelID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Release>()
                .HasRequired(r => r.Genre)
                .WithMany()
                .HasForeignKey(r => r.GenreID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Song>()
                .HasRequired(s => s.Genre)
                .WithMany()
                .HasForeignKey(s => s.GenreID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Page>()
                .HasMany(m => m.Images)
                .WithRequired()
                .HasForeignKey(s => s.PageId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Release>().
              HasMany(c => c.Songs).
              WithMany(p => p.News);

            modelBuilder.Entity<SEO>().Property(p => p.SEO_ID)
            .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}
