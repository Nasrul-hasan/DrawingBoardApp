const canvas = document.getElementById("boardCanvas");
const ctx = canvas.getContext("2d");

const colorPicker = document.getElementById("colorPicker");
const brushSize = document.getElementById("brushSize");
const penBtn = document.getElementById("penBtn");
const eraserBtn = document.getElementById("eraserBtn");
const clearBoardBtn = document.getElementById("clearBoardBtn");

let drawing = false;
let currentStroke = [];
let lastPoint = null;
let currentTool = "pen";
let savedStrokes = [];

function resizeCanvas() {
    const rect = canvas.getBoundingClientRect();

    canvas.width = rect.width;
    canvas.height = rect.height;

    redrawAllStrokes();
}

function redrawAllStrokes() {
    ctx.clearRect(0, 0, canvas.width, canvas.height);

    savedStrokes.forEach(stroke => {
        drawFullStroke(stroke);
    });
}

resizeCanvas();
window.addEventListener("resize", resizeCanvas);

function setTool(tool) {
    currentTool = tool;

    penBtn.classList.remove("active-tool");
    eraserBtn.classList.remove("active-tool");

    canvas.classList.remove("pen-cursor");
    canvas.classList.remove("eraser-cursor");

    if (tool === "pen") {
        penBtn.classList.add("active-tool");
        canvas.classList.add("pen-cursor");
    } else {
        eraserBtn.classList.add("active-tool");
        canvas.classList.add("eraser-cursor");
    }
}

penBtn.addEventListener("click", () => setTool("pen"));
eraserBtn.addEventListener("click", () => setTool("eraser"));
setTool("pen");

function getPoint(e) {
    const rect = canvas.getBoundingClientRect();

    return {
        x: e.clientX - rect.left,
        y: e.clientY - rect.top
    };
}

function getCurrentColor() {
    return currentTool === "eraser" ? "#ffffff" : colorPicker.value;
}

function getCurrentSize() {
    const size = Number(brushSize.value);
    return currentTool === "eraser" ? size * 3 : size;
}

function drawSegment(p1, p2, color, size) {
    ctx.globalCompositeOperation = "source-over";
    ctx.strokeStyle = color;
    ctx.lineWidth = Number(size);
    ctx.lineCap = "round";
    ctx.lineJoin = "round";

    ctx.beginPath();
    ctx.moveTo(p1.x, p1.y);
    ctx.lineTo(p2.x, p2.y);
    ctx.stroke();
}

function drawLastSegment(points, color, size) {
    if (points.length < 2) return;

    const p1 = points[points.length - 2];
    const p2 = points[points.length - 1];

    drawSegment(p1, p2, color, size);
}

function drawFullStroke(stroke) {
    if (!stroke || !stroke.points || stroke.points.length < 2) return;

    for (let i = 1; i < stroke.points.length; i++) {
        drawSegment(
            stroke.points[i - 1],
            stroke.points[i],
            stroke.color,
            stroke.size
        );
    }
}

canvas.addEventListener("mousedown", (e) => {
    drawing = true;
    currentStroke = [];

    const point = getPoint(e);
    currentStroke.push(point);
    lastPoint = point;
});

canvas.addEventListener("mousemove", (e) => {
    if (!drawing) return;

    const point = getPoint(e);

    const dx = point.x - lastPoint.x;
    const dy = point.y - lastPoint.y;
    const distance = Math.sqrt(dx * dx + dy * dy);

    if (distance < 3) return;

    currentStroke.push(point);
    drawLastSegment(currentStroke, getCurrentColor(), getCurrentSize());

    lastPoint = point;
});

canvas.addEventListener("mouseup", () => {
    drawing = false;

    if (currentStroke.length > 1) {
        sendStroke(currentStroke);
    }
});

canvas.addEventListener("mouseleave", () => {
    if (drawing && currentStroke.length > 1) {
        sendStroke(currentStroke);
    }

    drawing = false;
});

const connection = new signalR.HubConnectionBuilder()
    .withUrl("/boardHub")
    .build();

connection.start()
    .then(() => {
        connection.invoke("JoinBoard", window.boardConfig.boardId.toString());
        console.log("Connected to board hub");
    })
    .catch(err => console.error(err));

function sendStroke(points) {
    const stroke = {
        points: points,
        color: getCurrentColor(),
        size: getCurrentSize(),
        tool: currentTool,
        createdBy: window.boardConfig.nickname
    };

    // save locally immediately so resize does not remove the user's own latest stroke
    savedStrokes.push(stroke);

    connection.invoke("SendStroke", window.boardConfig.boardId.toString(), stroke)
        .catch(err => console.error(err));
}

connection.on("ReceiveStroke", (stroke) => {
    savedStrokes.push(stroke);
    drawFullStroke(stroke);
});

if (clearBoardBtn) {
    clearBoardBtn.addEventListener("click", () => {
        const confirmed = confirm("Are you sure you want to clear this board?");

        if (!confirmed) return;

        savedStrokes = [];
        ctx.clearRect(0, 0, canvas.width, canvas.height);

        connection.invoke("ClearBoard", window.boardConfig.boardId.toString())
            .catch(err => console.error(err));
    });
}

connection.on("BoardCleared", () => {
    savedStrokes = [];
    ctx.clearRect(0, 0, canvas.width, canvas.height);
});