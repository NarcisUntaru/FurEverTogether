Pet Adoption & Rescue Platform (FurEver Together) Full-stack pet adoption management system for connecting shelters with potential adopters through personality-based matching.

Stack: C#/.NET 8 ASP.NET Core MVC + Entity Framework Core + MySQL + Razor Views

Features:

- Configurable pet listings with type-based filtering (Dog, Cat, Bird, Small Mammal, Fish) and sorting
- Personality-based matching algorithm using Euclidean distance across 9 personality dimensions (Energy, Sociability, Affection, Trainability, Playfulness, Aggression, Noise, Adaptability, Anxiety)
- Full adoption lifecycle management: request, approve, reject with automatic email notifications
- Shelter management with capacity tracking and map-ready JSON API endpoints
- Volunteer application system with duplicate prevention and admin approval workflow
- Statistical dashboard on home page showing adoption metrics (current year + all-time totals)
- PDF report export of all adoption requests with styled status badges
- Contact/support form with admin email forwarding


Architecture: ASP.NET Core MVC backend with Repository + Service + Unit of Work layers. Controllers are thin; business logic encapsulated in services; data access abstracted through generic/specific repositories. Entity Framework Core with owned entity types for personality profiles. AutoMapper for ViewModel/DataModel mapping. Identity scaffolded for authentication with custom personality profile capture during registration.

