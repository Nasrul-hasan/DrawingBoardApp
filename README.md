# DrawingBoardApp Collaborative Drawing Board

## Overview

This is a real-time collaborative drawing web application built with ASP.NET Core MVC.  
Multiple users can join the same board and draw together simultaneously. All drawings are saved and remain available even after refreshing or rejoining the board.

The goal of this project was to build something that feels like a real product, not just a demo — focusing on usability, responsiveness, and smooth collaboration.

---

## Features

- Create and join drawing boards without authentication
- Real-time collaboration using SignalR (WebSockets)
- Multiple users can draw on the same board simultaneously
- All drawings are stored in the database and persist over time
- Canvas automatically resizes with the window
- Simple and intuitive drawing tools:
  - Pen tool
  - Eraser tool
  - Color picker
  - Brush size control
  - Clear board
- Clean and responsive UI

---

## How it works

Each board acts as a shared drawing space.

- When a user draws, the stroke data is sent to the server
- The server broadcasts it to all connected users in the same board
- The drawing is also saved in the database as JSON
- When a user joins a board, all previous drawings are loaded and rendered

---

## Technologies used

- ASP.NET Core MVC
- SignalR (for real-time communication)
- Entity Framework Core
- JavaScript (Canvas API)
- HTML / CSS

---

## Running the project

1. Open the solution in Visual Studio
2. Run database migration (if needed):
3. Run the application
4. Open in browser

---

## Deployment

The project is deployed here:

👉 

---

## Demo video

👉 

---

## Notes

- No authentication is used — users simply enter a nickname
- Drawings are stored per board and persist across sessions
- Designed to be lightweight and responsive

---

## Future improvements

- Multiple pages per board
- Shape tools (rectangle, circle, text)
- Board thumbnails
- Export to image
- Permissions system

---

## Author

Nasrul Hasan
