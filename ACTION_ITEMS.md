# Action Items - Fitness Project Microservices

**Priority Guide:**
- 🔴 **CRITICAL**: Must fix before any production deployment
- 🟡 **HIGH**: Fix within current sprint
- 🟢 **MEDIUM**: Plan for next sprint
- 🔵 **LOW**: Nice to have

---

## 🔴 CRITICAL PRIORITY

### 1. Add JWT Authentication to All Services
**Status:** ❌ Missing on 5 out of 7 services  
**Impact:** CRITICAL SECURITY VULNERABILITY - All user data is unprotected  
**Effort:** 2-4 hours per service

**Services Needing Authentication:**
- [ ] WorkoutCatalogService
- [ ] NutritionService  
- [ ] UserProfileService
- [ ] UserTrainingTrackingService

**Files to Modify:**
```
/WorkoutCatalogService/Program.cs
/NutritionService/Program.cs
/UserProfileService/Program.cs
/UserTrainingTrackingService/Program.cs
```

**Implementation Checklist:**
- [ ] Add JWT Bearer authentication configuration
- [ ] Add UseAuthentication() before UseAuthorization()
- [ ] Add [Authorize] attributes to all controllers
- [ ] Configure JWT settings in appsettings.json
- [ ] Test authentication with valid/invalid tokens

**Code Template:**
```csharp
// Add to Program.cs:
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
        };
    });

// In middleware pipeline (correct order):
app.UseAuthentication();  // BEFORE UseAuthorization
app.UseAuthorization();
```

---

### 2. Fix Sync-Over-Async Deadlock Risk
**Status:** ❌ Present in all RabbitMQ initialization code  
**Impact:** Application can hang on startup  
**Effort:** 3-4 hours

**Files to Fix (12 total):**
- [ ] WorkoutCatalogService/Shared/MessageBrocker/MessageBrokerService/MessageBrokerPublisher.cs
- [ ] WorkoutCatalogService/Shared/MessageBrocker/MessageBrokerService/RabbitMQConsumerService.cs
- [ ] UserProfileService/Shared/MessageBrocker/MessageBrokerService/MessageBrokerPublisher.cs
- [ ] UserProfileService/Shared/MessageBrocker/MessageBrokerService/RabbitMQConsumerService.cs
- [ ] NutritionService/Shared/MessageBrocker/MessageBrokerService/MessageBrokerPublisher.cs
- [ ] NutritionService/Shared/MessageBrocker/MessageBrokerService/RabbitMQConsumerService.cs

**Problem Code:**
```csharp
// ❌ WRONG - Causes deadlock risk
_connection = _factory.CreateConnectionAsync().Result;
_channel = _connection.CreateChannelAsync().Result;
```

**Solution:**
```csharp
// ✅ CORRECT - Async initialization
public static class MessageBrokerPublisherFactory
{
    public static async Task<MessageBrokerPublisher> CreateAsync(
        ILogger<MessageBrokerPublisher> logger,
        IConfiguration configuration)
    {
        var publisher = new MessageBrokerPublisher(logger, configuration);
        await publisher.InitializeAsync();
        return publisher;
    }
}

private async Task InitializeAsync()
{
    try
    {
        _connection = await _factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();
        _logger.LogInformation("RabbitMQ connection established");
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Failed to connect to RabbitMQ");
        throw;
    }
}
```

---

### 3. Add Rate Limiting to API Gateway
**Status:** ❌ Not implemented  
**Impact:** Vulnerable to abuse and DDoS attacks  
**Effort:** 1-2 hours

**Files to Modify:**
- [ ] ApiGateway/Program.cs
- [ ] ApiGateway/appsettings.json

**Implementation:**
```csharp
// In Program.cs, add after AddReverseProxy():
builder.Services.AddRateLimiter(options =>
{
    // Default policy for public endpoints
    options.AddFixedWindowLimiter("default", opt =>
    {
        opt.PermitLimit = 100;
        opt.Window = TimeSpan.FromMinutes(1);
        opt.QueueLimit = 0;
    });
    
    // Strict policy for authentication endpoints
    options.AddSlidingWindowLimiter("auth", opt =>
    {
        opt.PermitLimit = 5;
        opt.Window = TimeSpan.FromMinutes(1);
        opt.SegmentsPerWindow = 4;
        opt.QueueLimit = 0;
    });
});

// In middleware pipeline:
app.UseRateLimiter();  // BEFORE MapReverseProxy
app.MapReverseProxy();
```

**Update appsettings.json:**
```json
"Routes": {
  "IdentityServiceRoute": {
    "RateLimiterPolicy": "auth",  // Add this line
    "ClusterId": "IdentityServiceRouteCluster",
    "Match": { "Path": "/Identity/{**catch-all}" },
    "Transforms": [{ "PathRemovePrefix": "/Identity" }]
  },
  "WorkoutCatalogServiceRoute": {
    "RateLimiterPolicy": "default",  // Add this line
    // ... rest of config
  }
}
```

---

### 4. Move JWT Secret to Environment Variables
**Status:** ❌ Secret key hardcoded in appsettings.json  
**Impact:** Security risk - secret in source control  
**Effort:** 30 minutes

**Files to Modify:**
- [ ] IdentityService/appsettings.json
- [ ] IdentityService/Program.cs
- [ ] Create `.gitignore` entry for secrets

**Steps:**
1. Remove key from appsettings.json:
```json
{
  "JWT": {
    "Issuer": "https://yourapp.com",
    "Audience": "https://yourapp.com",
    "Key": "",  // Remove actual key
    "ExpirationInMinutes": 15
  }
}
```

2. Use User Secrets for development:
```bash
cd IdentityService
dotnet user-secrets init
dotnet user-secrets set "JWT:Key" "your-secret-key-at-least-32-characters-long"
```

3. Add validation in Program.cs:
```csharp
var jwtKey = builder.Configuration["JWT:Key"] 
    ?? throw new InvalidOperationException(
        "JWT:Key not configured. Use User Secrets (dev) or Environment Variables (prod)");

if (jwtKey.Length < 32)
    throw new InvalidOperationException("JWT:Key must be at least 32 characters");
```

4. For production, use environment variables:
```bash
export JWT__Key="production-secret-key"
```

---

## 🟡 HIGH PRIORITY

### 5. Add Pagination to All List Endpoints
**Status:** ⚠️ Only implemented in NutritionService  
**Impact:** Performance issues with large datasets  
**Effort:** 2-3 hours

**Endpoints Needing Pagination:**
- [ ] GET /WorkoutCatalog/api/Category/GetAllCategories
- [ ] GET /WorkoutCatalog/api/Workout/GetAllWorkouts
- [ ] GET /WorkoutCatalog/api/Plan/GetAllPlans

**Files to Modify:**
```
WorkoutCatalogService/Features/Categories/CQRS/Quries/GetAllCategories.cs
WorkoutCatalogService/Features/Workout/CQRS/Queries/GetAllWorkoutsQuery.cs
WorkoutCatalogService/Features/Plans/CQRS/Quries/GetAllplansCommend.cs
```

**Implementation Pattern:**
```csharp
// 1. Create shared PagedResult class (in Shared folder):
public class PagedResult<T>
{
    public List<T> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPrevious => PageNumber > 1;
    public bool HasNext => PageNumber < TotalPages;
}

// 2. Update query to accept pagination parameters:
public record GetAllWorkoutsQuery(int PageNumber = 1, int PageSize = 20)
    : IRequest<RequestResponse<PagedResult<WorkoutToreturnDto>>>;

// 3. Update handler:
public async Task<RequestResponse<PagedResult<WorkoutToreturnDto>>> Handle(...)
{
    var query = genericRepository.GetAll().Include(x => x.SubCategory);
    
    var totalCount = await query.CountAsync();
    
    var workouts = await query
        .Skip((request.PageNumber - 1) * request.PageSize)
        .Take(request.PageSize)
        .Select(w => new WorkoutToreturnDto { ... })
        .ToListAsync();
    
    return RequestResponse<PagedResult<WorkoutToreturnDto>>.Success(
        new PagedResult<WorkoutToreturnDto>
        {
            Items = workouts,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        });
}
```

---

### 6. Implement Caching Strategy
**Status:** ⚠️ Only registered in IdentityService, not used  
**Impact:** Unnecessary database load on read-heavy endpoints  
**Effort:** 3-4 hours

**Priority Caching Targets:**
- [ ] WorkoutCatalogService - Categories (rarely change)
- [ ] WorkoutCatalogService - Workouts (reference data)
- [ ] NutritionService - Meal catalog
- [ ] IdentityService - Roles and permissions

**Files to Modify:**
```
WorkoutCatalogService/Program.cs - Add caching service
WorkoutCatalogService/Features/Categories/CQRS/Quries/GetAllCategories.cs
WorkoutCatalogService/Features/Workout/CQRS/Queries/GetAllWorkoutsQuery.cs
NutritionService/Program.cs
```

**Implementation:**

**Step 1: Add caching to Program.cs**
```csharp
// For single instance (in-memory):
builder.Services.AddMemoryCache();

// For distributed/multi-instance (Redis):
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["Redis:ConnectionString"];
    options.InstanceName = "WorkoutCatalog_";
});
```

**Step 2: Use in handlers (cache-aside pattern)**
```csharp
public class GetAllCategoriesHandler : IRequestHandler<...>
{
    private readonly IGenericRepository<category> _categoryRepo;
    private readonly IMemoryCache _cache;
    private const string CacheKey = "AllCategories";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(30);
    
    public GetAllCategoriesHandler(
        IGenericRepository<category> categoryRepo,
        IMemoryCache cache)
    {
        _categoryRepo = categoryRepo;
        _cache = cache;
    }
    
    public async Task<RequestResponse<IEnumerable<CategoriesDTO>>> Handle(...)
    {
        // Try get from cache
        if (_cache.TryGetValue(CacheKey, out IEnumerable<CategoriesDTO> cachedData))
        {
            return RequestResponse<IEnumerable<CategoriesDTO>>.Success(cachedData);
        }
        
        // Cache miss - query database with projection
        var categories = await _categoryRepo.GetAll()
            .Select(c => new CategoriesDTO
            {
                Name = c.Name,
                Description = c.Description,
                SubCategories = c.SubCategories.Select(sc => new SubCategoryDTo
                {
                    Name = sc.Name,
                    Description = sc.Description
                }).ToList()
            })
            .ToListAsync();
        
        // Store in cache
        _cache.Set(CacheKey, categories, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = CacheDuration,
            SlidingExpiration = TimeSpan.FromMinutes(10)
        });
        
        return RequestResponse<IEnumerable<CategoriesDTO>>.Success(categories);
    }
}
```

**Step 3: Cache invalidation (for write operations)**
```csharp
// In AddCategoryOrchestrator or command handlers:
public class AddCategoryOrchestrator : IRequestHandler<...>
{
    private readonly IMemoryCache _cache;
    
    public async Task<RequestResponse<bool>> Handle(...)
    {
        // Add category logic...
        
        // Invalidate cache
        _cache.Remove("AllCategories");
        
        return RequestResponse<bool>.Success(true);
    }
}
```

**Recommended Cache Durations:**
- Categories: 30-60 minutes (rarely change)
- Workouts: 30 minutes
- Meals: 15-30 minutes
- Roles/Permissions: 10-15 minutes

---

### 7. Optimize Queries with Projection
**Status:** ⚠️ Manual mapping after ToListAsync()  
**Impact:** Performance degradation, memory overhead  
**Effort:** 1-2 hours

**Files to Optimize:**
- [ ] WorkoutCatalogService/Features/Categories/CQRS/Quries/GetAllCategories.cs (lines 22-51)
- [ ] WorkoutCatalogService/Features/Workout/CQRS/Queries/GetAllWorkoutsQuery.cs (lines 21-42)

**Problem:**
```csharp
// ❌ Inefficient - loads full entities then maps in C#
var Categories = await _categoryRepo.GetAll()
    .Include(x => x.SubCategories)
    .ToListAsync();

// Then manual mapping with loops
foreach (var Category in Categories)
{
    foreach (var sc in Category.SubCategories)
    {
        // ...
    }
}
```

**Solution:**
```csharp
// ✅ Efficient - project in database
var categories = await _categoryRepo.GetAll()
    .Select(c => new CategoriesDTO
    {
        Name = c.Name,
        Description = c.Description,
        SubCategories = c.SubCategories.Select(sc => new SubCategoryDTo
        {
            Name = sc.Name,
            Description = sc.Description
        }).ToList()
    })
    .ToListAsync();

// No manual mapping needed, single optimized SQL query
return RequestResponse<IEnumerable<CategoriesDTO>>.Success(categories);
```

**Benefits:**
- Single SQL query with joins
- Less data transferred from database
- Less memory allocation
- Faster execution

---

## 🟢 MEDIUM PRIORITY

### 8. Complete Incomplete Services
**Status:** ❌ Three services are placeholders  
**Impact:** Missing functionality  
**Effort:** 1-2 weeks per service

**Services to Complete:**
- [ ] UserTrainingTrackingService (has domain, needs features implementation)
- [ ] NotificationService (needs full implementation)
- [ ] ReportsService (needs full implementation)

**For each service:**
1. Remove WeatherForecast template files
2. Create Features folder
3. Implement CQRS commands and queries
4. Add controllers with proper authorization
5. Add unit tests

---

### 9. Add Health Checks
**Status:** ❌ Not implemented  
**Impact:** No visibility into service health  
**Effort:** 1-2 hours

**Files to Modify:**
- [ ] All service Program.cs files
- [ ] ApiGateway/Program.cs (aggregate health checks)

**Implementation:**
```csharp
// In each service Program.cs:
builder.Services.AddHealthChecks()
    .AddDbContextCheck<YourDbContext>("database")
    .AddRabbitMQ(
        rabbitConnectionString: "amqp://admin:admin123@rabbitmq:5672",
        name: "rabbitmq");

// Map endpoint:
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var result = JsonSerializer.Serialize(new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description
            })
        });
        await context.Response.WriteAsync(result);
    }
});
```

**At Gateway (aggregate):**
```csharp
builder.Services.AddHealthChecksUI()
    .AddInMemoryStorage();

app.MapHealthChecksUI(options => options.UIPath = "/health-ui");
```

---

### 10. Add Correlation IDs for Distributed Tracing
**Status:** ❌ Not implemented  
**Impact:** Difficult to trace requests across services  
**Effort:** 2-3 hours

**Create Middleware:**
```csharp
// Shared/Middlewares/CorrelationIdMiddleware.cs
public class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;
    private const string CorrelationIdHeader = "X-Correlation-ID";
    
    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task InvokeAsync(HttpContext context, ILogger<CorrelationIdMiddleware> logger)
    {
        var correlationId = context.Request.Headers[CorrelationIdHeader].FirstOrDefault()
            ?? Guid.NewGuid().ToString();
        
        context.Response.Headers.Add(CorrelationIdHeader, correlationId);
        
        using (logger.BeginScope(new Dictionary<string, object>
        {
            ["CorrelationId"] = correlationId
        }))
        {
            await _next(context);
        }
    }
}

// In Program.cs:
app.UseMiddleware<CorrelationIdMiddleware>();
```

---

## 🔵 LOW PRIORITY (Quick Wins)

### 11. Remove Template Files
**Effort:** 5 minutes per service

**Files to Delete:**
- [ ] All WeatherForecastController.cs files
- [ ] All WeatherForecast.cs model files

```bash
# Quick cleanup:
find . -name "WeatherForecast*.cs" -delete
```

---

### 12. Add Database Indexes
**Effort:** 30 minutes

**Create migration for indexes:**

**IdentityService:**
```csharp
migrationBuilder.CreateIndex(
    name: "IX_Users_Email",
    table: "Users",
    column: "Email",
    unique: true);

migrationBuilder.CreateIndex(
    name: "IX_RefreshTokens_Token",
    table: "RefreshTokens",
    column: "Token");
```

**WorkoutCatalogService:**
```csharp
migrationBuilder.CreateIndex(
    name: "IX_Workout_SubCategoryId",
    table: "Workout",
    column: "SubCategoryId");
```

**NutritionService:**
```csharp
migrationBuilder.CreateIndex(
    name: "IX_Meals_MealType",
    table: "Meals",
    column: "MealType");

migrationBuilder.CreateIndex(
    name: "IX_Ingredients_MealId",
    table: "Ingredients",
    column: "MealId");
```

---

### 13. Standardize Error Responses
**Effort:** 1-2 hours

**Create shared error model:**
```csharp
public class ErrorResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public List<string> Errors { get; set; } = new();
    public string CorrelationId { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
```

**Use in exception middleware:**
```csharp
catch (Exception ex)
{
    var response = new ErrorResponse
    {
        StatusCode = 500,
        Message = "An error occurred",
        Errors = new List<string> { ex.Message },
        CorrelationId = correlationId
    };
    
    await context.Response.WriteAsJsonAsync(response);
}
```

---

## Summary

**Total Items:** 13  
**Critical:** 4 (MUST FIX BEFORE PRODUCTION)  
**High:** 3 (Fix this sprint)  
**Medium:** 3 (Plan for next sprint)  
**Low:** 3 (Quick wins)

**Estimated Total Effort:** 2-3 weeks for all critical and high priority items

**Next Steps:**
1. Start with Critical Priority items 1-4
2. Add automated tests for authentication flow
3. Move to High Priority items
4. Review and iterate

**Remember:** Don't skip the critical items. They represent fundamental security and stability issues that could cause serious problems in production.
