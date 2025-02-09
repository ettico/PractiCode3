using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using TodoApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// הוספת DbContext
builder.Services.AddDbContext<ToDoDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("ToDoDB"), 
    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("ToDoDB"))));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("AllowAll");

// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();
// }

// הגדרת ה-routes
app.MapGet("/items", async (ToDoDbContext db) =>
{
    return await db.Items.ToListAsync(); // שליפת כל המשימות
});

app.MapPost("/items", async (ToDoDbContext db, Item item) =>
{
    db.Items.Add(item); // הוספת משימה חדשה
    await db.SaveChangesAsync();
    return Results.Created($"/items/{item.Id}", item);
});

// app.MapPut("/items/{id}", async (int id, ToDoDbContext db, Item updatedItem) =>
// {
//     var item = await db.Items.FindAsync(id);
//     if (item is null) return Results.NotFound();

//     item.Name = updatedItem.Name; // עדכון משימה
//     item.IsComplete = updatedItem.IsComplete;
    
//     await db.SaveChangesAsync();
//     return Results.NoContent();
// });

app.MapPut("/items/{id}", (int id, Item updatedItem, ToDoDbContext db) =>
{
    var item = db.Items.Find(id);
    if (item is null) return Results.NotFound();
   
    // עדכון רק את הסטטוס אם הוא נשלח
    if (updatedItem.IsComplete.HasValue)
    {
        item.IsComplete = updatedItem.IsComplete;
    }
    if (!string.IsNullOrEmpty(updatedItem.Name))
    {
        item.Name = updatedItem.Name;
    }
   
    db.SaveChanges();
    return Results.NoContent();
});

app.MapDelete("/items/{id}", async (int id, ToDoDbContext db) =>
{
    var item = await db.Items.FindAsync(id);
    if (item is null) return Results.NotFound();

    db.Items.Remove(item); // מחיקת משימה
    await db.SaveChangesAsync();
    return Results.NoContent();
});
app.MapGet("/",()=>"TodoApi-server is running");
app.Run();



