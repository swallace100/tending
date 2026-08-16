# Tending

A grounding and self-care tool for people living with Complex PTSD (C-PTSD).

**[Play on itch.io](https://iyasustudio.itch.io/tending)**, a free, browser-based, works on PC and mobile.

## About

C-PTSD results from prolonged and repeated trauma, such as childhood abuse, domestic violence, captivity, and most existing mental health apps and research are built around PTSD, not C-PTSD, which has different needs. Tending focuses specifically on C-PTSD.

Tending is not a replacement for a mental health professional. It's a tool for self-care. It has five short activities that help someone ground and relax in the moment. Each is designed around C-PTSD research and mental health professional recommendations, and each takes about 5 minutes or less. See [RESEARCH.md](RESEARCH.md) for the clinical background and sources.

No personal data is collected or saved. The only data saved between sessions is volume settings.

## Activities

- **Body Relaxation**: Progressive muscle relaxation, tensing and releasing muscle groups in sequence.
- **Grounding**: Sensory grounding techniques to bring attention back to the present.
- **Paced Breathing**: A guided breathing pace to help downregulate fight or flight response.
- **Self Check-In**: A quick check-in to the user's physical and emotional states (hunger, temperature, tension, loneliness, etc.) to help them pinpoint which need isn't being met.
- **Self Friend**: A self-compassion exercise to reflect on what you'd say to a friend in the same situation.

## How it's built

Tending is built in Unity (C#, uGUI) and runs in-browser via WebGL. Each activity is its own self-contained controller under `Assets/Scripts/Games`, so activities can be added or revised independently, with a shared event system handling navigation between the main menu, activity selection, and individual activities. Accessibility (text scaling, adjustable options) was built in from the start rather than added later, since usability matters as much as content for a self-care tool.

Almost all art and audio assets are free/licensed assets — full credits are on the [itch.io page](https://iyasustudio.itch.io/tending).

## License

Original source code is MIT licensed — see [LICENSE](LICENSE). Third-party art, audio, and font assets keep their own licenses; see the itch.io page for credits.

## Contributing & Feedback

This is a solo hackathon project I'd like to keep improving with feedback from mental health professionals, artists, and people with lived experience of C-PTSD. Feel free to open an issue if you have suggestions.
