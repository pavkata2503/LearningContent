﻿using Learning_Content_Models.Models;
using Learning_Content_Models.Models.Enums;
using Microsoft.AspNetCore.Identity;

namespace Learning_Content_Models.Data
{
	public class DbSeeder
	{
		public static async Task SeedRolesAndAdminAsync(IServiceProvider service)
		{
			//Seed Roles
			var userManager = service.GetService<UserManager<ApplicationUser>>();
			var roleManager = service.GetService<RoleManager<IdentityRole>>();
			await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
			await roleManager.CreateAsync(new IdentityRole(Roles.Teacher.ToString()));
			await roleManager.CreateAsync(new IdentityRole(Roles.Student.ToString()));

			// creating admin

			var user = new ApplicationUser
			{
				UserName = "admin@gmail.com",
				Email = "admin@gmail.com",
				Name = "Павката",
				FirstName = "Павел",
				LastName = "Петров",
				Description = "Това е администратор",
				PhoneNumber = "0884390393",
				EmailConfirmed = true,
				PhoneNumberConfirmed = true
			};

			var user1 = new ApplicationUser
			{
				UserName = "vladislav@gmail.com",
				Email = "vladislav@gmail.com",
				Name = "Владо",
				FirstName = "Владислав",
				LastName = "Христов",
				Description = "Това е създаден учител. Той е преподавател по история",
				PhoneNumber = "0884390393",
				EmailConfirmed = true,
				PhoneNumberConfirmed = true
			};

			var userInDb = await userManager.FindByEmailAsync(user.Email);
			if (userInDb == null)
			{
				await userManager.CreateAsync(user, "Admin@123");
				await userManager.AddToRoleAsync(user, Roles.Admin.ToString());
			}
			var userInDb1 = await userManager.FindByEmailAsync(user1.Email);
			if (userInDb1 == null)
			{
				await userManager.CreateAsync(user1, "Vladislav123!");
				await userManager.AddToRoleAsync(user1, Roles.Admin.ToString());
			}
		}
		public async Task CreateMaterialSeeder(ApplicationDbContext context)
		{
			if (context.StudyMaterials.Any())
			{
				return; // Database has been seeded
			}

			var studymaterials = new List<StudyMaterial>
			{
				new StudyMaterial
				{
					Title = "Първа световна война",
					CreatedByName = "Ваката",
					URL = "https://p.nationalgeographic.bg/4/7/471_pic_big-6943-1140x0.jpg",
					Description = "Тове е урок, в който ще се запознаем с основната информация относно Първата световна война.",
					Category = Category.NewMaterial,
					TypeFile = TypeFile.Image,
					Subject = "История",
					Class = 12,
					CreateDate = new DateTime(2024, 4, 11, 10, 0, 0),
					FileTitle= "093c7ca4-a129-4196-a74e-01da4392c36f.pptx"
				},
				new StudyMaterial
				{
					Title = "Втора световна война",
					CreatedByName = "Николай",
					URL = "https://trud.bg/public/images/articles/2019-06/30-5-2_4767003951340255203_original.jpg",
					Description = "Тове е урок, в който ще се запознаем с основната информация относно Втората световна война. Той е предназначен за хора, които ще се подготвят за тест.",
					Category = Category.ForTesting,
					TypeFile = TypeFile.TextDocument,
					Subject = "История",
					Class = 11,
					CreateDate = new DateTime(2024, 6, 4, 12, 0, 0),
					FileTitle= "4a37b453-8da6-45d1-9cf8-f175d7dc4e30.jpg"
				},
				new StudyMaterial
				{
					Title = "Дроби",
					CreatedByName = "Владо",
					URL = "https://i.ytimg.com/vi/uvXrFDKOs0w/hq720.jpg?sqp=-oaymwEhCK4FEIIDSFryq4qpAxMIARUAAAAAGAElAADIQj0AgKJD&rs=AOn4CLALKLSr732yWflFEPm57IGxeQoaig",
					Description = "Тове е урок, в който ще се запознаем с Дробите. Той е предназначен за хора, които ще се качат задачите за домашно.",
					Category = Category.Homework,
					TypeFile = TypeFile.TextDocument,
					Subject = "Математика",
					Class = 8,
					CreateDate = new DateTime(2024, 9, 10, 1, 0, 0),
					FileTitle= "c324b467-db3c-4f97-a92b-2ba146bc8efb.jpg"
				}
			};
			// Add cars to the database
			context.StudyMaterials.AddRange(studymaterials);
			context.SaveChanges();
		}
	}
}
