# NutritionTracker

A clean, modern **mobile** app built with **.NET 9** and **.NET MAUI** for tracking calories and macronutrients. This minimalistic application includes all the **essential features** you need to manage your food intake and monitor your nutrition goals on the go.

> 💡 Includes a barcode scanner using Open Food Facts and powerful sorting and customization features.

---

## 📁 Project Structure

This repository contains multiple projects to ensure a clean separation of concerns:

| Project                   | Description                                                                 |
| ------------------------- | --------------------------------------------------------------------------- |
| `NutritionTracker`        | The main .NET MAUI application (Android)                                    |
| `BusinessLayer`           | Handles all CRUD operations by directly using the DataLayer                 |
| `DataLayer`               | SQLite-based data access layer using `sqlite-net-pcl`                       |
| `NutritionTrackerConsole` | Console version of the app (no GUI or barcode scanner, minimal functionality) |
| `NutritionTrackerTests`   | Unit tests using NUnit for testing the business layer                        |

---

## 📦 Building the APK

### Steps

1. Set the configuration to **Release** (not Debug)
2. Right-click the `NutritionTracker` project in Visual Studio
3. Select **Publish
4. Follow the prompts to generate and save your `.apk` file

---

## 📱 How to Use the App

The app is structured around **four main pages**, accessible via the bottom navigation bar:

### 🏠 Home

- Displays today's **macronutrient summary**
- Lists meals logged for today
- **Tap** a meal to edit it
- **Swipe left** to delete a meal

### 🍎 Foods

- Shows all foods in the local database
- **Swipe right** to edit a food
- **Swipe left** to delete a food
- **Add Food** opens the Food Detail Page

### 📅 History

- Lists previous logged days
- **Tap** a day to open the **Meal Day Page**
- **Swipe left** to delete a logged day

### ⚙️ Settings

- **Change Accent Color** (teal, mint, scarlet, coral, etc.)
- **Sort Foods** (A–Z, Z–A, or by nutrient value — highest to lowest or lowest to highest)
- **Danger Zone Options**:
  - Delete entire database
  - Clear meal history (keep food DB)

---

### 🔄 Secondary Pages

These are pages that are navigated to from the main tabs:

#### 🍽️ Meal Detail Page

- Add/remove foods for a specific meal
- View live macro updates
- Accessed by tapping a meal on **Home** or **Meal Day Page**

#### 📝 Food Detail Page

- Create or edit a food entry
- Set name, calories, protein, fat, carbs per 100g
- Accessed via **Foods Page**
- **Tap "Scan Barcode"** to go to the **Barcode Scanner Page**

#### 🔍 Barcode Scanner Page

- Scan product barcodes to fetch food data from **Open Food Facts**
- Accessed from the **Food Detail Page**

#### 📆 Meal Day Page

- View meals and macros for a previously logged day
- Same layout and functionality as the **Home Page**
- Accessed from the **History Page**
