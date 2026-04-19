using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;

namespace ProyectoFinal_BibliotecaPersonal.Drawables;

public class StatisticsDrawable : IDrawable
{
    public int TotalBooks { get; set; }
    public int ReadBooks { get; set; }
    public int UnreadBooks { get; set; }
    public Dictionary<string, int> BooksByGenre { get; set; } = new Dictionary<string, int>();

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        // 1. Gráfico circular: Leídos vs Pendientes
        DrawPieChart(canvas, dirtyRect);

        // 2. Gráfico de barras: Libros por género
        DrawBarChart(canvas, dirtyRect);
        
        // 3. Estadísticas numéricas grandes
        DrawStatNumbers(canvas, dirtyRect);
    }

    private void DrawPieChart(ICanvas canvas, RectF dirtyRect)
    {
        int total = ReadBooks + UnreadBooks;
        if (total == 0) return;

        // Situar en la parte superior izquierda
        float cx = dirtyRect.Width * 0.25f;
        float cy = dirtyRect.Height * 0.25f;
        float radius = Math.Min(dirtyRect.Width, dirtyRect.Height) * 0.15f;

        float readAngle = (ReadBooks / (float)total) * 360f;
        float unreadAngle = 360f - readAngle;

        // Dibujar el sector de "Leídos" (Verde)
        canvas.FillColor = Colors.Green;
        canvas.FillArc(cx - radius, cy - radius, radius * 2, radius * 2, 90f, 90f + readAngle, true);

        // Dibujar el sector de "Pendientes" (Gris claro)
        canvas.FillColor = Colors.LightGray;
        canvas.FillArc(cx - radius, cy - radius, radius * 2, radius * 2, 90f + readAngle, 90f + readAngle + unreadAngle, true);

        // Texto explicativo (Leyenda)
        canvas.FontColor = Colors.Black;
        canvas.FontSize = 14f;
        canvas.DrawString($"Leídos: {ReadBooks}", cx, cy + radius + 20, HorizontalAlignment.Center);
        canvas.DrawString($"Pendientes: {UnreadBooks}", cx, cy + radius + 40, HorizontalAlignment.Center);
    }

    private void DrawBarChart(ICanvas canvas, RectF dirtyRect)
    {
        if (BooksByGenre == null || BooksByGenre.Count == 0) return;

        // Situar en la parte inferior
        float startX = 40f;
        float chartWidth = dirtyRect.Width - 80f;
        float chartHeight = dirtyRect.Height * 0.35f;
        float startY = dirtyRect.Height * 0.9f; 

        int maxBooks = 0;
        foreach (var count in BooksByGenre.Values)
        {
            if (count > maxBooks) maxBooks = count;
        }

        if (maxBooks == 0) return;

        int numBars = BooksByGenre.Count;
        float barSpacing = 15f;
        float barWidth = (chartWidth - (barSpacing * (numBars + 1))) / numBars;

        canvas.FontColor = Colors.Black;
        canvas.FontSize = 16f;
        canvas.DrawString("Libros por Género", dirtyRect.Width / 2, startY - chartHeight - 25, HorizontalAlignment.Center);

        float currentX = startX + barSpacing;
        int index = 0;
        Color[] colors = { Colors.Blue, Colors.Orange, Colors.Purple, Colors.Magenta, Colors.Cyan };

        foreach (var kvp in BooksByGenre)
        {
            float height = (kvp.Value / (float)maxBooks) * chartHeight;
            float topY = startY - height;

            canvas.FillColor = colors[index % colors.Length];
            canvas.FillRectangle(currentX, topY, barWidth, height);

            // Nombre del género (abajo de la barra)
            canvas.FontColor = Colors.Black;
            canvas.FontSize = 12f;
            canvas.DrawString(kvp.Key, currentX + barWidth / 2, startY + 5, HorizontalAlignment.Center);

            // Valor numérico (encima de la barra)
            canvas.DrawString(kvp.Value.ToString(), currentX + barWidth / 2, topY - 15, HorizontalAlignment.Center);

            currentX += barWidth + barSpacing;
            index++;
        }
    }

    private void DrawStatNumbers(ICanvas canvas, RectF dirtyRect)
    {
        // Situar en la parte superior derecha
        float xOffset = dirtyRect.Width * 0.75f;
        float yOffset = dirtyRect.Height * 0.15f;

        // Total de libros
        canvas.FontColor = Colors.Black;
        canvas.FontSize = 16f;
        canvas.DrawString("Total de Libros", xOffset, yOffset, HorizontalAlignment.Center);
        
        canvas.FontSize = 42f;
        canvas.FontColor = Colors.Navy;
        canvas.DrawString(TotalBooks.ToString(), xOffset, yOffset + 30, HorizontalAlignment.Center);

        // Porcentaje leído
        float percentage = TotalBooks > 0 ? (ReadBooks / (float)TotalBooks) * 100 : 0;
        
        float currentYOffset = yOffset + 90;
        canvas.FontSize = 16f;
        canvas.FontColor = Colors.Black;
        canvas.DrawString("% Leído", xOffset, currentYOffset, HorizontalAlignment.Center);

        canvas.FontSize = 32f;
        canvas.FontColor = percentage >= 50f ? Colors.Green : Colors.Orange;
        canvas.DrawString($"{percentage:F1}%", xOffset, currentYOffset + 30, HorizontalAlignment.Center);
    }
}