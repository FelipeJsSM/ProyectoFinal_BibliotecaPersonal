# 📚 Mi Biblioteca Personal - .NET MAUI

Una aplicación móvil desarrollada con arquitectura MVVM para gestionar una biblioteca personal, consumir APIs externas y visualizar estadísticas de lectura en tiempo real.

## 👨‍💻 Equipo de Desarrollo
* **Felipe:** UI/UX, Navegación (AppShell) e Integración General.
* **Radi:** Conexión API (Google Books) y Servicios HTTP.
* **Daybel:** Lógica de dibujo y Gráficos Estadísticos (GraphicsView).
* **Esmeralda:** Arquitectura MVVM, ViewModels y Bindings.
* **Xavier:** Persistencia de datos y configuración SQLite (DatabaseService).

## 🚀 Características Principales
* **CRUD Local:** Agregar, editar y eliminar libros usando SQLite.
* **Búsqueda Web:** Integración con la API de Google Books para importar libros reales.
* **Dashboard Estadístico:** Gráficos dinámicos interactivos sobre géneros y progreso de lectura.

## ⚠️ Nota Técnica para la Evaluación (Known Issue)
Al revisar el código fuente en Visual Studio, es posible que el **Diseñador Visual XAML** arroje una excepción (`0x8000FFFF`) al abrir `StatisticsPage.xaml`. 
* Este **no es un error del código**, sino un bug del diseñador de VS al intentar renderizar la clase personalizada `StatisticsDrawable` en tiempo de diseño. 
* El proyecto compila y se ejecuta al 100% sin errores. Recomendamos abrir el archivo usando *Right Click -> Open With... -> Source Code (Text) Editor*.
