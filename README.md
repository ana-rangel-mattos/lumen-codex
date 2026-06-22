# LumenCodex

LumenCodex is a powerful local-first media and education web application built to render and play locally hosted courses. It features a polished, intuitive interface inspired by major e-learning platforms like Udemy, allowing users to transform raw local directories into an organized, rich learning environment.

## 🚀 Tech Stack

- **Frontend:** React, TypeScript, Tailwind
- **Backend:** ASP.NET Core
- **Media Handling:** HTML5 Video API, WebVTT for subtitles
- **Content Rendering:** Markdown parser, HTML sandboxing

## ✨ Key Features

- **Udemy-like Workspace:** A seamless, responsive layout with an interactive sidebar for course navigation (sections, lectures, and resource types).
- **Multi-Format Rendering:** Supports native video playback alongside rich text documentation running side-by-side (`.md` and `.html` integration).
- **Polyglot Subtitles:** Advanced multi-language closed captions (`.vtt` / `.srt`) support with dynamic switching.
- **Local File Processing:** Efficient handling of local directories and files without needing to upload heavy media to external servers.

## 🗺️ Roadmap (Active Development)

The core playback engine is stable, and the following features are currently being built to enhance the study experience:
- [ ] **Interactive In-Class Notes:** A rich-text note-taking panel synchronized with each specific lecture.
- [ ] **Smart Video Timestamps:** Ability to bookmark precise timestamps within a video, allowing users to save and jump straight back to critical explanations.
- [ ] **Progress Persistence:** Global State synchronization to track watched videos and automatically resume from where the user left off.

## 🛠️ How to Run Locally

1. **Clone this Repo:**
```bash
git clone https://github.com/ana-rangel-mattos/lumen-codex.git
```

2. **Run Docker Image**
```bash
cd lumen-codex
docker-compose up --build -d
```
