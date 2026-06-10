using LibraryManagement.Business.Services.Abstract;
using LibraryManagement.Business.Services.Concrete;
using FluentValidation;
using FluentValidation.AspNetCore;
using LibraryManagement.Business.Mappings;
using LibraryManagement.Business.Validators;
using LibraryManagement.DataAccess.Context;
using LibraryManagement.DataAccess.Repositories.Abstract;
using LibraryManagement.DataAccess.Repositories.Concrete;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(BookProfile).Assembly));

builder.Services.AddValidatorsFromAssemblyContaining<BookCreateDtoValidator>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IPublisherRepository, PublisherRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IPublisherService, PublisherService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
