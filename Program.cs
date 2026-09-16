using DigitalSignatureSystem.Services;

var builder = WebApplication.CreateBuilder(args);

// ============================================
// 1. REGISTER SERVICES
// ============================================

builder.Services.AddControllers();

// DigitalSignatureService dùng Singleton
// để RSA key pair được giữ trong suốt thời gian
// ứng dụng đang chạy.
builder.Services.AddSingleton<DigitalSignatureService>();

// ============================================
// 2. SWAGGER
// ============================================

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// ============================================
// 3. BUILD APPLICATION
// ============================================

var app = builder.Build();


// ============================================
// 4. DEVELOPMENT / SWAGGER
// ============================================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// ============================================
// 5. STATIC FILES
// ============================================

// Cho phép chạy giao diện HTML trong wwwroot
app.UseDefaultFiles();
app.UseStaticFiles();


// ============================================
// 6. HTTPS
// ============================================

// Project hiện tại đang chạy HTTP ở localhost:5178,
// nên không ép chuyển sang HTTPS.
// Nếu sau này cấu hình HTTPS thì có thể bật lại:
// app.UseHttpsRedirection();


// ============================================
// 7. CONTROLLERS
// ============================================

app.MapControllers();


// ============================================
// 8. START APPLICATION
// ============================================

app.Run();