using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using Sample.Models;

namespace Sample.Migrations
{
    [DbContext(typeof(ModelContext))]
    [Migration("20151127070648_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348");

            modelBuilder.Entity("Sample.Models.Account.Account", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("Mail");

                    b.Property<string>("Name");

                    b.Property<int>("StatusType");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("Sample.Models.Account.Login", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("LoginId");

                    b.Property<string>("Password");

                    b.HasKey("Id");
                });
        }
    }
}
