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

    private static readonly StatusBadge PrivateProject = new(new("Özel proje", "Private project"), StatusTone.Private);

    /// <summary>
    /// Headline proof. Commits are mine on the default branches of Mevora, the LED platform, Motor,
    /// ClearPay and LED Teknik Destek, counted from GitHub on 2026-10-30. Update by hand;
    /// private repositories can't be read at runtime.
    /// </summary>
    public static readonly IReadOnlyList<ProofStat> ProofStats =
    [
        new(4, "", new("öne çıkan ürün", "featured products")),
        new(1457, "", new("commit", "commits")),
        new(2600, "+", new("otomatik test tek üründe", "automated tests in one product"))
    ];

    public static readonly IReadOnlyList<CaseStudy> CaseStudies =
    [
        new()
        {
            Id = "mevora",
            Name = "Mevora",
            Demo = "_DemoMevora",
            Accent = "--violet",
            Tagline = new(
                "İki kişinin neden uyumlu olduğunu açıkça gösteren, kesin konumu gizli tutan, iOS ve Android için bir tanışma uygulaması.",
                "A dating app for iOS and Android that shows exactly why two people are compatible and keeps precise location private."),
            Metrics =
            [
                new(283, "", new("Flutter test dosyası", "Flutter test files")),
                new(120, "", new("Cloud Functions test dosyası", "Cloud Functions test files")),
                new(716, "", new("commit", "commits"))
            ],
            Problem = new(
                "Çoğu tanışma uygulaması kişileri kapalı kutu skorlarla sıralar ve gerekenden fazla kişisel veri açar. Mevora uyumun nedenini gösterir, kesin konumu saklar ve güvenlik açısından kritik kuralları istemciden alıp sunucuya taşır.",
                "Most dating apps rank people with opaque scores and expose more personal data than they need to. Mevora shows why two people match, hides exact location and moves trust-critical rules off the client."),
            Contribution = new(
                "Tek geliştirici olarak Flutter uygulamasını, TypeScript Cloud Functions backend'ini, Firestore ve Storage güvenlik kurallarını ve ASP.NET Core destek sitesini tasarlayıp geliştiriyorum.",
                "As the sole developer I design and build the Flutter app, the TypeScript Cloud Functions backend, the Firestore and Storage security rules, and the companion ASP.NET Core support site."),
            Highlights =
            [
                new(new("Açıklanabilir uyum", "Explainable compatibility"),
                    new("Profil, müzik (şarkı %40 · sanatçı %30 · tür %20 · son dinlenen %10) ve ilişki sorusu skorları sunucuda hesaplanır. \"Yapay zekâ eşleştirmesi\" iddiası yok.",
                        "Profile, music (tracks 40% · artists 30% · genres 20% · recent 10%) and relationship-question scores are computed on the server. No \"AI matching\" claims.")),
                new(new("Sunucu otoriteli eşleşme", "Server-authoritative matching"),
                    new("Keşif akışı, kaydırmalar ve karşılıklı eşleşmeler Cloud Functions'ta üretilir; yarıçap, karşılıklı tercih, aktiflik ve engel filtreleri sunucuda uygulanır.",
                        "The discovery feed, swipes and mutual matches are produced by Cloud Functions, with radius, mutual-preference, activity and block filters applied server-side.")),
                new(new("Gizlilik ve güvenlik", "Privacy and safety"),
                    new("Kesin GPS'i yalnızca sahibi okuyabilir, diğerleri mesafe etiketi görür. Şikâyet, engelleme, mesaj hız limitleri, fotoğraf moderasyonu ve üçüncü taraf sağlayıcıyla canlılık selfie kontrolü.",
                        "Exact GPS is readable only by its owner; everyone else sees a distance label. Reporting, blocking, message rate limits, photo moderation and a liveness selfie check through a third-party provider.")),
                new(new("Şifreli sohbet", "Encrypted chat"),
                    new("Metin, görsel, GIF ve sesli mesajlar cihazda X25519 + AES-GCM ile şifrelenir; Firestore ve Storage şifreli veri tutar. Aramalar bu kapsamda değil.",
                        "Text, image, GIF and voice messages are encrypted on the device with X25519 + AES-GCM, so Firestore and Storage hold ciphertext. Calls are out of scope.")),
                new(new("Satın alma ve hesap hakları", "Purchases and account rights"),
                    new("Sunucu taraflı makbuz doğrulamasıyla Boost uygulama içi satın alımları; hesap silme ve veri dışa aktarma.",
                        "Boost in-app purchases with server-side receipt verification, plus account deletion and personal data export.")),
                new(new("Test ve CI", "Tests and CI"),
                    new("283 Flutter test dosyası, 120 Cloud Functions test dosyası ve emülatörde çalışan güvenlik kuralı testleri.",
                        "283 Flutter test files, 120 Cloud Functions test files and emulator-based security-rules tests."))
            ],
            Stack = ["Flutter", "Dart", "Firebase Auth", "Cloud Firestore", "Cloud Functions · TypeScript", "FCM", "Spotify Web API", "ASP.NET Core", "GitHub Actions"],
            Statuses = [PrivateProject, new(new("Lansman öncesi", "Pre-launch"), StatusTone.Active)],
            PrivateNote = new("Kaynak kodu özel bir depoda; burada ürün ve mühendislik özeti yer alıyor.",
                "The source lives in a private repository; this is a product and engineering summary.")
        },
        new()
        {
            Id = "led-platform",
            Name = "LED B2B Platform",
            Demo = "_DemoWall",
            Accent = "--accent",
            Tagline = new(
                "LED ekran almak isteyen firmalar için yapılandırma, teklif ve siparişi tek çatıda toplayan B2B platform. Tedarikçiler ve maliyetleri müşteriye görünmez.",
                "A B2B platform that brings configuration, quotes and orders for LED screens under one roof, with suppliers and their costs hidden from the customer."),
            Metrics =
            [
                new(2600, "+", new("otomatik test", "automated tests")),
                new(10, "", new("tamamlanan geliştirme fazı", "completed development phases")),
                new(440, "", new("commit", "commits"))
            ],
            Problem = new(
                "Bir LED ekran almak; piksel aralığı, kabin, kontrol kartı, güç ve kurulumu farklı tedarikçiler arasında eşleştirmek demek. Platform bunu tek bir yapılandırılmış, fiyatlandırılmış ve yönetilen siparişe dönüştürüyor.",
                "Buying an LED wall means matching pixel pitch, cabinets, controllers, power and installation across several suppliers. The platform turns that into one configured, priced and managed order."),
            Contribution = new(
                "Alan modelini, yapılandırma ve fiyatlandırma motorlarını, .NET 10 üzerindeki API ve MVC uygulamasını tasarlayıp geliştirdim.",
                "I designed and built the domain model, the configuration and pricing engines, and the API and MVC application on .NET 10."),
            Highlights =
            [
                new(new("Yapılandırma ve fiyat motoru", "Configuration and pricing engines"),
                    new("Müşterinin ihtiyacını teknik bir LED yapılandırmasına, sonra da fiyatlandırılmış bir teklife çeviren ayrı motorlar.",
                        "Separate engines turn a customer's need into a technical LED configuration, then into a priced quote.")),
                new(new("Tiplerle korunan fiyatlandırma", "Pricing safety through types"),
                    new("Tedarikçi maliyeti, platform marjı ve müşteri fiyatı ayrı value object'ler; maliyet ile satış fiyatı yanlışlıkla karışamaz.",
                        "Supplier cost, platform margin and customer price are distinct value objects, so cost and sale price can't be mixed up by accident.")),
                new(new("Clean Architecture", "Clean Architecture"),
                    new("Domain, Application, Infrastructure, Persistence ve API katmanları; dış servisler port/adapter arkasında.",
                        "Domain, Application, Infrastructure, Persistence and API layers, with external services behind ports and adapters.")),
                new(new("Güvenlik temeli", "Security baseline"),
                    new("JWT doğrulama, rol politikalı ASP.NET Identity (platform yöneticisi, personel, tedarikçi, müşteri), güvenlik başlıkları, üretimde gizlenen hata detayları.",
                        "JWT validation, ASP.NET Identity with role policies (platform admin, staff, supplier, customer), security headers and no error details in production responses.")),
                new(new("Test", "Tests"),
                    new("Tam test paketi PostgreSQL ile yerelde koşuyor: son kayıtlı sonuç 2.691 başarılı, 0 başarısız.",
                        "The full suite runs locally against PostgreSQL: the last recorded result is 2,691 passed, 0 failed."))
            ],
            Implemented =
            [
                new("Yapılandırma ve fiyatlandırma motorları", "Configuration and pricing engines"),
                new("Kimlik, roller ve API altyapısı", "Identity, roles and API foundation"),
                new("Ödeme akışı (simülatörde test edildi)", "Payment flow (tested against a simulator)"),
                new("2.600+ otomatik test", "2,600+ automated tests")
            ],
            Roadmap =
            [
                new("Gerçek ödeme altyapısıyla test", "Testing with a real payment provider"),
                new("Vergi ve e-fatura", "Tax and e-invoicing"),
                new("Yönetim göstergeleri", "Admin KPIs"),
                new("Yasal sayfalar", "Legal pages")
            ],
            Stack = [".NET 10", "ASP.NET Core", "EF Core", "PostgreSQL", "ASP.NET Identity", "JWT", "xUnit"],
            Statuses = [PrivateProject, new(new("Genel erişime kapalı", "Not publicly available"), StatusTone.Neutral)],
            PrivateNote = new("Kaynak kodu ve ürün genel erişime kapalı; burada yalnızca ürün ve mimari özeti yer alıyor.",
                "The source and product aren't publicly available; this is a product and architecture summary.")
        },
        new()
        {
            Id = "motor",
            Name = "Motor",
            Demo = "_DemoMotor",
            Accent = "--ember",
            Tagline = new(
                "Motosiklet parçası satan bir işletme için e-ticaret sitesi: ürün kataloğu, sepet, ödeme ve yönetim paneli bir arada; parçalar marka, model ve yıla göre aranır.",
                "An e-commerce site for a motorcycle parts business, with catalogue, cart, checkout and admin panel in one place, and parts searched by make, model and year."),
            Metrics =
            [
                new(10, "", new("MotorShop modülü", "MotorShop modules")),
                new(823, "", new("test senaryosu", "test cases")),
                new(213, "", new("commit", "commits"))
            ],
            Problem = new(
                "Motosiklet parçasında doğru ürün; motorun markasına, modeline ve yılına bağlı. Müşterinin motoruna uyan parçayı hızlıca bulup tek yerden satın alabilmesi gerekiyor.",
                "With motorcycle parts, the right product depends on the bike's make, model and year. Customers need to find what fits their bike quickly and buy it in one place."),
            Contribution = new(
                "İşletmeye özel e-ticaret sitesini geliştiriyorum: MotorShop modülleri, MotorDark teması, uyumlu parça araması ve ödeme akışı.",
                "I'm building the business's e-commerce site: the MotorShop modules, the MotorDark theme, fitment search and the payment flow."),
            Highlights =
            [
                new(new("Uyumlu parça araması", "Fitment search"),
                    new("Parçalar marka, model ve yıl üzerinden motosiklete bağlanır.",
                        "Parts are linked to motorcycles by make, model and year.")),
                new(new("Modüler geliştirme", "Modular development"),
                    new("Mağazaya özel işlevler 10 ayrı MotorShop modülünde ve MotorDark temasında toplanıyor.",
                        "Store-specific features live in 10 separate MotorShop modules and the MotorDark theme."))
            ],
            Implemented =
            [
                new("10 MotorShop modülü ve MotorDark teması", "10 MotorShop modules and the MotorDark theme"),
                new("Marka, model ve yıla göre parça araması", "Search by make, model and year"),
                new("Ödeme modülü (mock testlerle)", "Payment module (tested with mocks)")
            ],
            Roadmap =
            [
                new("Gerçek ödeme altyapısıyla test", "Testing with a real payment provider"),
                new("İşletme verilerinin tamamlanması", "Completing the business data"),
                new("Canlı ortama geçiş", "Going to production")
            ],
            Stack = [".NET 10", "ASP.NET Core", "EF Core", "PostgreSQL", "Docker"],
            Statuses = [new(new("Müşteri projesi", "Client project"), StatusTone.Neutral), new(new("Demo aşaması", "Demo stage"), StatusTone.Active)],
            Links = [new(new("Arayüz önizlemesi", "Interface preview"), "https://motor-vitrin.vercel.app/motor/")]
        },
        new()
        {
            Id = "led-support-bot",
            Name = "LED Support Bot",
            Demo = "_DemoBot",
            Accent = "--live",
            Tagline = new(
                "LED ekran teknik servisi için WhatsApp destek karşılama sistemi: teknisyenin ihtiyaç duyduğu bilgileri toplar, sonra konuşmayı bir insana devreder.",
                "A WhatsApp support-intake system for LED display service: it collects the details a technician needs, then hands the conversation to a human."),
            Metrics =
            [
                new(100, "", new(".NET testi", ".NET tests")),
                new(3, "", new("çalışma modu: bot, duraklatılmış, insan", "modes: bot, paused, human"))
            ],
            Problem = new(
                "Teknisyenler her müşteriye aynı soruları soruyordu: ekran ölçüsü, panel etiket fotoğrafı, receiver sayısı, AnyDesk ID. Bot bunları adım adım toplar; teknisyen işe eksiksiz bir vakayla başlar.",
                "Technicians kept asking every customer the same questions: screen size, panel label photos, receiver count, AnyDesk ID. The bot gathers these step by step, so a technician starts with a complete case."),
            Contribution = new(
                "Backend iş akışını, WhatsApp bağlayıcısını, kalıcılık katmanını ve test altyapısını tasarlayıp geliştirdim.",
                "I designed and built the backend workflow, the WhatsApp connector, the persistence layer and the test suite."),
            Highlights =
            [
                new(new("Backend'e ait iş akışı", "Backend-owned workflow"),
                    new("Slot tabanlı bir durum makinesi neyin ne zaman sorulacağına ve devrin ne zaman yapılacağına karar verir.",
                        "A slot-driven state machine decides what to ask next and when to hand off.")),
                new(new("Önce kural, sonra yapay zekâ", "Deterministic first, AI second"),
                    new("Ölçü, sayı ve ID'leri ayrıştırıcılar çözer. LLM yalnızca bir maliyet kapısı izin verdiğinde katı bir JSON şemasıyla çağrılır; çıktısı doğrulanır ve denetim kaydına yazılır. Yapay zekâ mesaj göndermez, durumu yönetmez.",
                        "Parsers handle sizes, counts and IDs. An LLM is called with a strict JSON schema only when a cost gate allows it, and its output is validated and audited. The AI never sends messages or owns state.")),
                new(new("İnsana devir", "Human takeover"),
                    new("Bot, duraklatılmış ve insan modları; teknisyen yazdığında bot otomatik olarak geri çekilir.",
                        "Bot, paused and human modes; when a technician replies, the bot steps back automatically.")),
                new(new("Güvenilir veri alımı", "Reliable ingest"),
                    new("İdempotent webhook (uygulama kontrolü + unique constraint), Node bağlayıcı ile .NET API arasında sabit zamanlı paylaşılan anahtar doğrulaması, medya türü doğrulaması.",
                        "Idempotent webhook ingest (app check plus unique constraint), constant-time shared-secret auth between the Node connector and the .NET API, and media type validation."))
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
                new("Operatör uygulamasının ana dala alınması", "Merging the operator app into the main branch"),
                new("Gerçek WhatsApp üzerinde uçtan uca test", "End-to-end testing on real WhatsApp"),
                new("Canlı ortam barındırma", "Production hosting")
            ],
            Stack = [".NET 8", "ASP.NET Core", "EF Core", "PostgreSQL", "Node.js", "LLM · JSON schema", "Testcontainers", "React · Vite"],
            Statuses = [PrivateProject, new(new("Test aşamasında", "In testing"), StatusTone.Active)],
            PrivateNote = new("Kaynak kodu özel bir depoda; müşteri verisi ve bağlantı bilgileri paylaşılmaz.",
                "The source lives in a private repository; customer data and connection details are not shared.")
        }
    ];

    public static readonly IReadOnlyList<ProjectCard> MoreProjects =
    [
        new()
        {
            Name = "LED Teknik Destek",
            RepoName = "ASP.NET-APP-FOR-LED",
            Category = new("LED sektörü · Web", "LED industry · Web"),
            Tags = "backend web led",
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
            Name = "ClearPay",
            RepoName = "clearpay",
            Category = new("Fintech · Backend", "Fintech · Backend"),
            Tags = "backend mobile",
            Summary = new(
                "Tek bir SQL Server çift taraflı defteri üzerinde ASP.NET Core 8 + Flutter demo cüzdan. Bakiye güncellenmez, kayıtlardan türetilir; transferler idempotent (tekrar → 409), outbox aynı transaction'da.",
                "A demo wallet on one SQL Server double-entry ledger, with an ASP.NET Core 8 web app and a Flutter client. Balance is derived, never updated; transfers are idempotent (replay → 409) and the outbox commits in the same transaction."),
            Stack = [".NET 8", "SQL Server", "Flutter", "Hangfire", "Docker", "CI"],
            Image = new("/img/projects/clearpay-summary.webp", new("ClearPay web uygulaması özet ekranı", "ClearPay web app summary screen"), 960, 733),
            Status = new(new("Açık kaynak demo", "Open-source demo"), StatusTone.Open)
        },
        new()
        {
            Name = "Task Management System",
            RepoName = "TaskManagementSystem",
            Category = new("Bitirme projesi", "Capstone project"),
            Tags = "backend",
            Summary = new(
                "Onion mimarili ASP.NET Core MVC görev yönetimi: Admin ve Member rolleri, MediatR, FluentValidation, EF Core + SQL Server, cookie kimlik doğrulama ve görev raporları.",
                "Onion-architecture ASP.NET Core MVC task manager: Admin and Member roles, MediatR, FluentValidation, EF Core + SQL Server, cookie auth and task reports."),
            Stack = ["ASP.NET Core MVC", "MediatR", "EF Core", "SQL Server"],
            Glyph = "mediator"
        },
        new()
        {
            Name = "Personal Finance Tracker",
            RepoName = "personal-Finance-Tracker",
            Category = new("Mobil", "Mobile"),
            Tags = "mobile",
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
            Tags = "mobile",
            Summary = new(
                "Jetpack Compose ve Material 3 ile yazılmış VPN uygulaması arayüz çalışması; aynı arayüzü Flutter'da da kurduğum karşılaştırmalı bir deneme.",
                "A VPN app interface study in Jetpack Compose and Material 3, built alongside a Flutter version of the same UI as a comparison."),
            Stack = ["Kotlin", "Jetpack Compose", "Material 3"],
            Glyph = "android"
        }
    ];

    /// <summary>Smaller and earlier repositories. Course work is labelled as learning, not presented as product work.</summary>
    public static readonly IReadOnlyList<ArchiveRepo> Archive =
    [
        new("FlutterVpnApp", "Flutter", new("Tasarımını ve kodunu kendim yaptığım VPN arayüzü", "A VPN interface I designed and coded myself"), "mobile"),
        new("fundemantal_of_biomedical_proejct", "Flutter", new("Okul projesi: hastalar için hatırlatma uygulaması", "School project: a reminder app for patients"), "mobile"),
        new("Sqflite-Fltuter-App", "Flutter", new("Sqflite ile yerel veri saklama", "Local storage with Sqflite"), "mobile"),
        new("ASP-NET-E-Trade", "ASP.NET", new("E-ticaret uygulaması", "E-commerce app"), "backend"),
        new("BankAppAsp", "ASP.NET", new("Bankacılık senaryosu çalışması", "A banking scenario exercise"), "backend"),
        new("NLayerdApp", "C#", new("N-katmanlı Windows uygulaması", "An N-layer Windows app"), "backend"),
        new("IdentityCourse", "ASP.NET", new("ASP.NET Identity kurs çalışması", "ASP.NET Identity course work"), "backend learning"),
        new("AllDesingPatternInBtkC-", "C#", new("BTK Akademi tasarım kalıpları kursu", "BTK Academy design patterns course"), "backend learning"),
        new("FirebaseChatApp", "Flutter", new("Öğrenme projesi: Firebase ile grup sohbeti kursu", "Learning project: a Firebase group chat course"), "mobile learning")
    ];

    public static readonly IReadOnlyList<SkillGroup> Skills =
    [
        new(new("Backend", "Backend"), new("Günlük işimin merkezi", "Where most of my work lives"),
            ["C#", ".NET 8 · 10", "ASP.NET Core", "Razor Pages · MVC", "REST APIs", "EF Core", "MediatR", "FluentValidation"]),
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
            new("Mevora'yı lansmana hazırlıyorum; LED B2B platformunu ve Motor e-ticaret sitesini geliştiriyorum.",
                "Preparing Mevora for launch and building the LED B2B platform and the Motor e-commerce site."),
            ["Flutter", "Cloud Functions", ".NET 10"])
    ];
}
