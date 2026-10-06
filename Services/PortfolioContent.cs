using Portfolio.Models;

namespace Portfolio.Services;

/// <summary>
/// Curated portfolio content. Every claim here is backed by the project repositories;
/// roadmap items are kept in separate lists so they are never presented as shipped.
/// </summary>
public static class PortfolioContent
{
    public const string FullName = "Halil Mert Develi";
    public const string Email = "halilmertdeveliii@gmail.com";
    public const string GitHubUrl = "https://github.com/HalilMertDeveli";
    public const string GitHubReposUrl = "https://github.com/HalilMertDeveli?tab=repositories";
    public const string LinkedInUrl = "https://www.linkedin.com/in/halil-mert-develi-00983a225/";
    public const string Portrait = "/img/profile/halil-mert-develi.webp";

    private static readonly StatusBadge ActiveDevelopment = new(new("Aktif geliştirme", "Active development"), StatusTone.Active);
    private static readonly StatusBadge PrivateProject = new(new("Özel proje", "Private project"), StatusTone.Private);

    public static readonly IReadOnlyList<CaseStudy> CaseStudies =
    [
        new()
        {
            Id = "mevora",
            Name = "Mevora",
            RepoName = "mevora",
            Visual = "_VisualMevora",
            Tagline = new(
                "Eşleşmeleri açıklanabilir sinyallere göre kuran, güvenlik ve konum gizliliğini sunucuda uygulayan Flutter + Firebase tanışma uygulaması.",
                "A Flutter + Firebase dating app that matches people on explainable signals, with safety and location privacy enforced on the server."),
            Problem = new(
                "Çoğu tanışma uygulaması kişileri kapalı kutu skorlarla sıralar ve gerekenden fazla kişisel veri açar. Mevora iki kişinin neden uyumlu olduğunu açıklar, kesin konumu gizli tutar ve güvenlik açısından kritik her kuralı istemciden alıp sunucuya taşır.",
                "Most dating apps rank people with opaque scores and expose more personal data than they need to. Mevora explains why two people match, keeps exact location private and moves every trust-critical rule off the client."),
            Contribution = new(
                "Tek geliştirici olarak Flutter uygulamasını, TypeScript Cloud Functions backend'ini, Firestore ve Storage güvenlik kurallarını ve ASP.NET Core destek ve yönetim sitelerini tasarlayıp geliştiriyorum.",
                "As the sole developer I design and build the Flutter app, the TypeScript Cloud Functions backend, the Firestore and Storage security rules, and the companion ASP.NET Core support and admin sites."),
            Highlights =
            [
                new(new("Açıklanabilir uyum", "Explainable compatibility"),
                    new("Profil, müzik (şarkı %40 · sanatçı %30 · tür %20 · son dinlenen %10) ve ilişki sorusu skorları sunucuda hesaplanır. \"Yapay zekâ eşleştirmesi\" iddiası yok.",
                        "Profile, music (tracks 40% · artists 30% · genres 20% · recent 10%) and relationship-question scores are computed on the server. No \"AI matching\" claims.")),
                new(new("Sunucu otoriteli eşleşme", "Server-authoritative matching"),
                    new("Keşif akışı, kaydırmalar ve karşılıklı eşleşmeler Cloud Functions'ta üretilir; yarıçap, karşılıklı tercih, aktiflik ve engel filtreleri sunucuda uygulanır.",
                        "The discovery feed, swipes and mutual matches are produced by Cloud Functions, with radius, mutual-preference, activity and block filters applied server-side.")),
                new(new("Gizlilik ve güvenlik", "Privacy and safety"),
                    new("Kesin GPS'i yalnızca sahibi okuyabilir, diğerleri mesafe etiketi görür. Şikâyet, engelleme, mesaj hız limitleri, fotoğraf moderasyon hattı ve canlılık selfie'siyle 1:1 yüz kontrolü.",
                        "Exact GPS is readable only by its owner; everyone else sees a distance label. Reporting, blocking, message rate limits, a photo moderation pipeline and a 1:1 face check against a liveness selfie.")),
                new(new("Uçtan uca şifreli sohbet", "End-to-end encrypted chat"),
                    new("Metin, görsel, GIF ve sesli mesajlar X25519 + AES-GCM ile şifrelenir; Firestore ve Storage yalnızca şifreli veri tutar.",
                        "Text, image, GIF and voice messages are encrypted with X25519 + AES-GCM; Firestore and Storage only ever hold ciphertext.")),
                new(new("Gelir modeli ve uyumluluk", "Monetization and compliance"),
                    new("Sunucu taraflı makbuz doğrulaması ve iade takibiyle Boost uygulama içi satın alımları; hesap silme ve veri dışa aktarma.",
                        "Boost in-app purchases with server-side receipt verification and refund handling; account deletion and personal data export.")),
                new(new("Test ve CI", "Tests and CI"),
                    new("280+ Flutter test dosyası, 120+ Cloud Functions testi ve emülatörde çalışan güvenlik kuralı testleri GitHub Actions'ta koşar.",
                        "280+ Flutter test files, 120+ Cloud Functions tests and emulator-based security-rules tests run on GitHub Actions."))
            ],
            Stack = ["Flutter", "Dart", "Firebase Auth", "Cloud Firestore", "Cloud Functions · TypeScript", "FCM", "Spotify Web API", "ASP.NET Core", "GitHub Actions"],
            Statuses = [ActiveDevelopment, new(new("Lansman öncesi", "Pre-launch"), StatusTone.Neutral)],
            Links = [new(new("Kaynak kodu", "Source code"), "https://github.com/HalilMertDeveli/mevora")]
        },
        new()
        {
            Id = "led-com",
            Name = "LED-COM",
            Visual = "_VisualLedCommerce",
            Tagline = new(
                "LED ekran sektörü için yönetilen B2B ticaret platformu: müşteri ekranı ve kurulumu tek markadan alır, tedarikçiler ve maliyetleri görünmez kalır.",
                "A managed B2B commerce platform for the LED display industry: customers buy screens and installation from one brand while suppliers and their costs stay hidden."),
            Problem = new(
                "Bir LED ekran almak; piksel aralığı, kabin, kontrol kartı, güç ve kurulumu farklı tedarikçiler arasında eşleştirmek demek. Platformun hedefi bunu tek bir yapılandırılmış, fiyatlandırılmış ve yönetilen siparişe dönüştürmek.",
                "Buying an LED wall means matching pixel pitch, cabinets, controllers, power and installation across several suppliers. The platform's goal is to turn that into one configured, priced and managed order."),
            Contribution = new(
                "Alan modelini tasarlıyor ve .NET 8 üzerinde mimari temeli kuruyorum: katmanlı çözüm, kimlik ve rol modeli, fiyat tipleri ve aşamalı yol haritası.",
                "I am designing the domain and laying the .NET 8 architecture foundation: the layered solution, the identity and role model, pricing types and a phased roadmap."),
            Highlights =
            [
                new(new("Clean Architecture", "Clean Architecture"),
                    new("Domain, Application, Infrastructure, Persistence ve API katmanları; port/adapter yapısı sayesinde uygulama veritabanı olmadan da ayağa kalkar.",
                        "Domain, Application, Infrastructure, Persistence and API layers with ports and adapters, so the app still boots without a database.")),
                new(new("Tiplerle korunan fiyatlandırma", "Pricing safety through types"),
                    new("Tedarikçi maliyeti, platform marjı ve müşteri fiyatı ayrı value object'ler; maliyet ile satış fiyatı yanlışlıkla karışamaz.",
                        "Supplier cost, platform margin and customer price are distinct value objects, so cost and sale price can't be mixed up by accident.")),
                new(new("Güvenlik temeli", "Security baseline"),
                    new("JWT doğrulama, rol politikalı ASP.NET Identity (platform yöneticisi, personel, tedarikçi, müşteri), güvenlik başlıkları, üretimde gizlenen hata detayları.",
                        "JWT validation, ASP.NET Identity with role policies (platform admin, staff, supplier, customer), security headers and no error details in production responses.")),
                new(new("Veri ve API", "Data and API"),
                    new("EF Core ile ayrı şemalı PostgreSQL (Supabase uyumlu), sürümlenmiş API, health check'ler ve xUnit testleri.",
                        "PostgreSQL through EF Core with a dedicated schema (Supabase-compatible), a versioned API, health checks and xUnit tests."))
            ],
            Implemented =
            [
                new("Katmanlı çözüm ve API altyapısı", "Layered solution and API foundation"),
                new("Identity şeması ve rol politikaları", "Identity schema and role policies"),
                new("Para ve fiyat value object'leri", "Money and pricing value objects"),
                new("Health check, API sürümleme, testler", "Health checks, API versioning, tests")
            ],
            Roadmap =
            [
                new("LED yapılandırma motoru: piksel aralığı, kabin, güç, kontrol kartı, malzeme listesi", "LED configuration engine: pixel pitch, cabinets, power, controllers, bill of materials"),
                new("Fiyatlandırma motoru ve teklifler", "Pricing engine and quotes"),
                new("Tedarikçi verisini gizleyen ürün kataloğu", "Product catalogue that hides supplier data"),
                new("Sipariş, ödeme, tedarik ve kurulum akışları", "Orders, payments, fulfilment and installation")
            ],
            Stack = [".NET 8", "ASP.NET Core", "EF Core", "PostgreSQL · Supabase", "ASP.NET Identity", "JWT", "FluentValidation", "xUnit"],
            Statuses = [PrivateProject, new(new("Temel aşaması", "Foundation phase"), StatusTone.Active)],
            PrivateNote = new("Kaynak kodu özel bir depoda; burada yalnızca ürün ve mimari özeti yer alıyor.",
                "The source lives in a private repository; this is a product and architecture summary.")
        },
        new()
        {
            Id = "led-support-bot",
            Name = "LED Support Bot",
            Visual = "_VisualLedSupportBot",
            Tagline = new(
                "LED ekran teknik servisi için WhatsApp destek karşılama sistemi: teknisyenin ihtiyaç duyduğu teşhis bilgilerini toplar, sonra konuşmayı bir insana devreder.",
                "A WhatsApp support-intake system for LED display service: it collects the diagnostic details a technician needs, then hands the conversation to a human."),
            Problem = new(
                "Teknisyenler her müşteriye aynı soruları soruyordu: ekran ölçüsü, panel etiket fotoğrafı, receiver sayısı, AnyDesk ID. Bot bunları adım adım toplar; teknisyen işe eksiksiz bir vakayla başlar.",
                "Technicians kept asking every customer the same questions: screen size, panel label photos, receiver count, AnyDesk ID. The bot gathers these step by step, so a technician starts with a complete case."),
            Contribution = new(
                "Backend iş akışını, WhatsApp bağlayıcısını, kalıcılık katmanını ve test altyapısını tasarlayıp geliştirdim.",
                "I designed and built the backend workflow, the WhatsApp connector, the persistence layer and the test suite."),
            Highlights =
            [
                new(new("Backend'e ait iş akışı", "Backend-owned workflow"),
                    new("11 aşamalı, slot tabanlı bir durum makinesi neyin ne zaman sorulacağına ve devrin ne zaman yapılacağına karar verir.",
                        "An 11-stage, slot-driven state machine decides what to ask next and when to hand off.")),
                new(new("Önce kural, sonra yapay zekâ", "Deterministic first, AI second"),
                    new("Ölçü, sayı ve ID'leri ayrıştırıcılar çözer. LLM yalnızca bir maliyet kapısı izin verdiğinde katı bir JSON şemasıyla çağrılır; çıktısı doğrulanır ve denetim kaydına yazılır. Yapay zekâ mesaj göndermez, durumu yönetmez.",
                        "Parsers handle sizes, counts and IDs. An LLM is called with a strict JSON schema only when a cost gate allows it, and its output is validated and audited. The AI never sends messages or owns state.")),
                new(new("İnsana devir", "Human takeover"),
                    new("Bot, duraklatılmış ve insan modları; teknisyen yazdığında bot otomatik olarak geri çekilir.",
                        "Bot, paused and human modes; when a technician replies, the bot steps back automatically.")),
                new(new("Güvenilir veri alımı", "Reliable ingest"),
                    new("İdempotent webhook (uygulama kontrolü + unique constraint), Node bağlayıcı ile .NET API arasında sabit zamanlı paylaşılan anahtar doğrulaması, medya türü doğrulaması.",
                        "Idempotent webhook ingest (app check plus unique constraint), constant-time shared-secret auth between the Node connector and the .NET API, and media type validation.")),
                new(new("Test ve CI", "Tests and CI"),
                    new("100+ .NET testi; entegrasyon testleri Testcontainers ile gerçek PostgreSQL üzerinde, GitHub Actions'ta koşar.",
                        "100+ .NET tests, with integration tests running against real PostgreSQL through Testcontainers on GitHub Actions."))
            ],
            Implemented =
            [
                new("WhatsApp bağlayıcısı (Node.js) ve ASP.NET Core API", "WhatsApp connector (Node.js) and ASP.NET Core API"),
                new("Konuşma iş akışı motoru ve ayrıştırıcılar", "Conversation workflow engine and parsers"),
                new("Kapılı ve doğrulanan yapay zekâ çıkarımı", "Gated, validated AI extraction"),
                new("Medya hattı ve PostgreSQL kalıcılığı", "Media pipeline and PostgreSQL persistence")
            ],
            Roadmap =
            [
                new("Operatör paneli: kuyruklar, devralma, özetler", "Operator dashboard: queues, takeover, summaries"),
                new("Teknisyen kimlik doğrulaması", "Technician authentication"),
                new("Gerçek zamanlı güncellemeler", "Realtime updates"),
                new("Canlı ortam barındırma", "Production hosting")
            ],
            Stack = [".NET 8", "ASP.NET Core", "EF Core", "PostgreSQL", "Node.js", "OpenAI structured outputs", "Testcontainers", "React · Vite", "GitHub Actions"],
            Statuses = [PrivateProject, ActiveDevelopment],
            PrivateNote = new("Kaynak kodu özel bir depoda; müşteri verisi ve bağlantı bilgileri paylaşılmaz.",
                "The source lives in a private repository; customer data and connection details are not shared.")
        }
    ];

    public static readonly IReadOnlyList<ProjectCard> MoreProjects =
    [
        new()
        {
            Name = "ClearPay",
            RepoName = "clearpay",
            Category = new("Fintech · Backend", "Fintech · Backend"),
            Summary = new(
                "Tek bir SQL Server çift taraflı defteri üzerinde ASP.NET Core 8 + Flutter demo cüzdan. Bakiye güncellenmez, kayıtlardan türetilir; transferler idempotent (tekrar → 409), outbox aynı transaction'da.",
                "A demo wallet on one SQL Server double-entry ledger, with an ASP.NET Core 8 web app and a Flutter client. Balance is derived, never updated; transfers are idempotent (replay → 409) and the outbox commits in the same transaction."),
            Stack = [".NET 8", "SQL Server", "Flutter", "Hangfire", "Docker", "CI"],
            Image = new("/img/projects/clearpay-summary.webp", new("ClearPay web uygulaması özet ekranı", "ClearPay web app summary screen"), 960, 733),
            Status = new(new("Açık kaynak", "Open source"), StatusTone.Open)
        },
        new()
        {
            Name = "LED Teknik Destek",
            RepoName = "ASP.NET-APP-FOR-LED",
            Category = new("LED sektörü · Web", "LED industry · Web"),
            Summary = new(
                "Colorlight, NovaStar ve Huidu kontrol sistemleri için canlıdaki destek sitesi: talep formu → Supabase PostgreSQL → Resend e-postası, Google ile giriş, gerçek zamanlı müşteri sohbeti ve yönetici paneli.",
                "A live support site for Colorlight, NovaStar and Huidu control systems: support form → Supabase PostgreSQL → Resend email, Google sign-in, realtime customer chat and an admin panel."),
            Stack = ["ASP.NET Core 8", "Razor Pages", "Supabase", "Resend", "Vercel"],
            Image = new("/img/projects/led-support-home.webp", new("LED Teknik Destek ana sayfası", "LED Teknik Destek home page"), 960, 600),
            Live = new(new("Canlı site", "Live site"), "https://asp-net-app-for-led.vercel.app"),
            Status = new(new("Canlıda", "Live"), StatusTone.Live)
        },
        new()
        {
            Name = "Task Management System",
            RepoName = "TaskManagementSystem",
            Category = new("Bitirme projesi", "Capstone project"),
            Summary = new(
                "Onion mimarili ASP.NET Core MVC görev yönetimi: Admin ve Member rolleri, MediatR, FluentValidation, EF Core + SQL Server, cookie kimlik doğrulama ve görev raporları.",
                "Onion-architecture ASP.NET Core MVC task manager: Admin and Member roles, MediatR, FluentValidation, EF Core + SQL Server, cookie auth and task reports."),
            Stack = ["ASP.NET Core MVC", "MediatR", "EF Core", "SQL Server"],
            Glyph = "mediator"
        },
        new()
        {
            Name = "Firebase Chat App",
            RepoName = "FirebaseChatApp",
            Category = new("Mobil · Gerçek zamanlı", "Mobile · Realtime"),
            Summary = new(
                "Firebase Auth ve Cloud Firestore ile Flutter grup sohbeti: grup oluşturma ve arama, üyelik, anlık mesaj akışı ve profil.",
                "A Flutter group chat on Firebase Auth and Cloud Firestore: creating and searching groups, membership, live message streams and profiles."),
            Stack = ["Flutter", "Firebase Auth", "Cloud Firestore"],
            Glyph = "chat"
        },
        new()
        {
            Name = "Personal Finance Tracker",
            RepoName = "personal-Finance-Tracker",
            Category = new("Mobil", "Mobile"),
            Summary = new(
                "Firebase Authentication (e-posta, Google, Apple), Firestore ve Storage kullanan, duyarlı yerleşimli Flutter kişisel finans uygulaması.",
                "A Flutter personal finance app with Firebase Authentication (email, Google, Apple), Firestore, Storage and responsive layouts."),
            Stack = ["Flutter", "Firebase Auth", "Firestore"],
            Glyph = "finance"
        },
        new()
        {
            Name = "VPN App · Compose",
            RepoName = "VPNAppWithCompose",
            Category = new("Android · Kotlin", "Android · Kotlin"),
            Summary = new(
                "Jetpack Compose ve Material 3 ile yazılmış VPN uygulaması arayüz çalışması; aynı arayüzü Flutter'da da kurduğum karşılaştırmalı bir deneme.",
                "A VPN app interface study in Jetpack Compose and Material 3, built alongside a Flutter version of the same UI as a comparison."),
            Stack = ["Kotlin", "Jetpack Compose", "Material 3"],
            Glyph = "android"
        }
    ];

    public static readonly IReadOnlyList<SkillGroup> Skills =
    [
        new(new("Backend", "Backend"), new("Günlük işimin merkezi", "Where most of my work lives"),
            ["C#", ".NET 8", "ASP.NET Core", "Razor Pages · MVC", "REST APIs", "EF Core", "MediatR", "FluentValidation"]),
        new(new("Mobil", "Mobile"), new("Flutter ağırlıklı, Android tecrübesiyle", "Flutter first, with Android experience"),
            ["Flutter", "Dart", "Firebase", "Kotlin", "Jetpack Compose", "go_router"]),
        new(new("Veri", "Data"), new("İlişkisel ve doküman tabanlı", "Relational and document stores"),
            ["PostgreSQL", "SQL Server", "Cloud Firestore", "Supabase", "Firebase Storage"]),
        new(new("Altyapı", "Infrastructure"), new("Geliştirmeden canlıya", "From local to production"),
            ["Docker", "Vercel", "Cloud Functions", "GitHub Actions", "Testcontainers", "Firebase Emulator Suite"]),
        new(new("Mimari ve mühendislik", "Architecture & engineering"), new("Kodun nasıl ayakta kaldığı", "How the code holds up"),
            ["Clean / Onion Architecture", "Domain modelling", "Authentication & authorization", "Security rules", "Idempotency", "Automated testing", "Third-party integrations"])
    ];

    public static readonly IReadOnlyList<Milestone> Journey =
    [
        new("2023", new("Mobil temeller", "Mobile foundations"),
            new("Kotlin ile Android (MVVM, Jetpack Compose), Java ve Spring Boot ile ilk backend denemeleri, Flutter ve Firebase ile gerçek zamanlı sohbet uygulaması.",
                "Android with Kotlin (MVVM, Jetpack Compose), first backend steps with Java and Spring Boot, and a realtime chat app with Flutter and Firebase."),
            ["Kotlin", "Java", "Flutter", "Firebase"]),
        new("2023 – 2024", new(".NET'e geçiş", "Moving to .NET"),
            new("N-katmanlı mimari, tasarım kalıpları, Entity Framework Core ve ASP.NET MVC ile e-ticaret ve bankacılık senaryoları.",
                "N-layer architecture, design patterns, Entity Framework Core and ASP.NET MVC through e-commerce and banking scenarios."),
            ["C#", "EF Core", "ASP.NET MVC"]),
        new("2025 – 2026", new("Mimari ve bitirme projesi", "Architecture and capstone"),
            new("ASP.NET Identity ile kimlik ve yetkilendirme; Bilgisayar Mühendisliği bitirme projesi olarak Onion mimarili, MediatR tabanlı görev yönetim sistemi.",
                "Identity and authorization with ASP.NET Identity, and an Onion-architecture, MediatR-based task management system as my Computer Engineering capstone."),
            ["ASP.NET Identity", "Onion Architecture", "MediatR"]),
        new("2026", new("LED sektöründe canlı yazılım", "Production software for the LED industry"),
            new("LED kontrol sistemleri için destek sitesi canlıya çıktı (Supabase, Resend, Vercel). Ardından çift taraflı defterli ClearPay ve WhatsApp destek botu geldi.",
                "Shipped a live support site for LED control systems (Supabase, Resend, Vercel), followed by the double-entry ClearPay wallet and the WhatsApp support bot."),
            ["Supabase", "Vercel", "PostgreSQL", "Node.js"]),
        new("2026 →", new("Ürün ölçeğinde işler", "Product-scale work"),
            new("Mevora'yı lansmana hazırlıyorum ve LED-COM B2B platformunun temelini kuruyorum.",
                "Preparing Mevora for launch and laying the foundation of the LED-COM B2B platform."),
            ["Flutter", "Cloud Functions", ".NET 8"])
    ];
}
