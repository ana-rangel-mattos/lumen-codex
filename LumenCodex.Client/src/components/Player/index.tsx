import * as React from "react";
import { useEffect, useRef, useState } from "react";
import type { Subtitle } from "../../types/ICourse.ts";
import { formatTime } from "../../utils/timeConvertion.ts";

interface IPlayerProps {
  videoUrl?: string;
  subtitles?: Subtitle[];
  lessonName: string;
}

const Player: React.FC<IPlayerProps> = ({
  videoUrl,
  subtitles,
  lessonName,
}) => {
  const playerRef = useRef<HTMLVideoElement | null>(null);

  const [isPlaying, setIsPlaying] = useState<boolean>(false);
  const [isMuted, setIsMuted] = useState<boolean>(false);
  const [currentTime, setCurrentTime] = useState<number>(0);
  const [duration, setDuration] = useState<number>(0);
  const [volume, setVolume] = useState<number>(0);
  const [playbackSpeed, setPlaybackSpeed] = useState<number>(1);
  const [videoName] = useState<string>(lessonName);

  useEffect(() => {
    const player = playerRef.current;

    if (!player) return;

    setIsPlaying(false);
    setCurrentTime(0);

    player.pause();
    player.load();
  }, [videoUrl]);

  function handleSeek(e: React.MouseEvent<HTMLDivElement>) {
    if (!playerRef.current || !duration) return;

    const rect = e.currentTarget.getBoundingClientRect();
    const clickX = e.clientX - rect.left;

    const width = rect.width;

    const percentage = clickX / width;

    playerRef.current.currentTime = percentage * duration;
  }

  function handleLoadVideo() {
    if (!playerRef.current) return;

    setDuration(playerRef.current.duration);
  }

  function handleTimeUpdate() {
    if (!playerRef.current) return;

    setCurrentTime(playerRef.current.currentTime);
  }

  function handlePlayPauseClick() {
    if (!playerRef.current) return;

    if (playerRef.current.paused) {
      playerRef.current.play().then();
      setIsPlaying(true);
    } else {
      playerRef.current.pause();
      setIsPlaying(false);
    }
  }

  function handleToggleMuteClick() {
    if (!playerRef.current) return;

    playerRef.current.muted = !playerRef.current.muted;
    setIsMuted(playerRef.current.muted);
  }

  function handleSkip(timeAmountSeconds: number) {
    if (!playerRef.current) return;

    playerRef.current.currentTime += timeAmountSeconds;
  }

  function handleVolumeChange(newVolume: number) {
    if (!playerRef.current) return;

    setVolume(newVolume);
    setIsMuted(newVolume === 0);
  }

  function handleSpeedChange(speed: number) {
    if (!playerRef.current) return;

    playerRef.current.playbackRate = speed;
    setPlaybackSpeed(speed);
  }

  useEffect(() => {
    const handleKeyDown = (e: KeyboardEvent) => {
      e.preventDefault();

      if (
        !playerRef.current ||
        e.target instanceof HTMLInputElement ||
        e.target instanceof HTMLTextAreaElement
      )
        return;

      switch (e.code) {
        case "Space":
          handlePlayPauseClick();
          break;
        case "ArrowLeft":
          handleSkip(-10);
          break;
        case "ArrowRight":
          handleSkip(10);
          break;
        case "ArrowUp":
          handleVolumeChange(volume + 0.05);
          break;
        case "ArrowDown":
          handleVolumeChange(volume - 0.05);
          break;
        case "KeyM":
          handleToggleMuteClick();
          break;
        case "KeyF":
          playerRef.current.requestFullscreen().then();
          break;
      }
    };

    window.addEventListener("keydown", handleKeyDown);

    return () => window.removeEventListener("keydown", handleKeyDown);
  }, [volume, isPlaying, isMuted]);

  return (
    <div className="group relative aspect-video max-h-200 w-full shadow-2xl">
      <h2 className="group-hover:bg-base-content/70 absolute w-full px-4 py-2 text-2xl text-white opacity-0 transition-opacity duration-300 group-hover:opacity-100 hover:opacity-100">
        {videoName}
      </h2>
      <video
        ref={playerRef}
        src={videoUrl}
        preload="metadata"
        onTimeUpdate={handleTimeUpdate}
        onLoadedMetadata={handleLoadVideo}
        onClick={handlePlayPauseClick}
        className="h-full w-full"
      >
        {subtitles?.map((subtitle) => (
          <track
            key={subtitle.subtitleId}
            src={subtitle.rootPath}
            kind="subtitles"
            srcLang={subtitle.languageCode}
          />
        ))}
      </video>

      {/*Customized Controls*/}
      <div className="absolute right-0 bottom-0 left-0 bg-gradient-to-t from-black/80 to-transparent px-4 py-3 opacity-0 transition-opacity duration-300 group-hover:opacity-100">
        {/* Progress Bar (Placeholder) */}
        <div
          onClick={handleSeek}
          className="group/progress relative mb-4 h-1.5 w-full cursor-pointer rounded-full bg-white/20 transition-all hover:h-2"
        >
          <div
            className="bg-accent absolute h-full rounded-full"
            style={{ width: `${(currentTime / duration) * 100}%` }}
          />
        </div>

        <div className="flex items-center justify-between">
          {/*LEFT SECTION*/}
          <div className="flex items-center space-x-4">
            {/* Play/Pause Button */}
            <button
              onClick={handlePlayPauseClick}
              className="text-white transition-transform hover:scale-110 active:scale-95"
            >
              {isPlaying ? (
                <i className="hn hn-pause-solid"></i>
              ) : (
                <i className="hn hn-play-solid"></i>
              )}
            </button>

            {/* Skip Button */}
            <button
              onClick={() => handleSkip(-10)}
              className="text-white hover:text-gray-300"
            >
              <i className="hn hn-arrow-alt-circle-left-solid"></i>
            </button>

            {/* Go back Button */}
            <button
              onClick={() => handleSkip(10)}
              className="text-white hover:text-gray-300"
            >
              <i className="hn hn-arrow-alt-circle-right-solid"></i>
            </button>

            {/*Current Time*/}
            <div className="font-mono tracking-tighter text-white hover:text-gray-300">
              {formatTime(currentTime)}{" "}
              <span className="mx-1 opacity-40">/</span> {formatTime(duration)}
            </div>
          </div>

          {/*MIDDLE SECTION*/}
          <div className="group/vol flex flex-1 items-center justify-center space-x-4">
            {/* Mute/Unmute Button */}
            <button
              onClick={handleToggleMuteClick}
              className="text-white opacity-80 hover:opacity-100"
            >
              {isMuted || volume === 0 ? (
                <i className="hn hn-sound-mute-solid"></i>
              ) : (
                <i className="hn hn-sound-on-solid"></i>
              )}
            </button>

            {/* Volume Control */}
            <input
              type="range"
              min={0}
              max={1}
              step={0.05}
              value={isMuted ? 0 : volume}
              onChange={(e) => handleVolumeChange(parseFloat(e.target.value))}
              className="accent-accent/80 h-1 w-24 cursor-pointer appearance-none rounded-lg bg-white/20"
            />
          </div>

          {/*RIGHT SECTION*/}
          <div className="flex flex-1 items-center justify-end space-x-6 text-white">
            {/*Playback Speed*/}
            <div className="group/speed relative font-mono text-xl">
              <button className="rounded border border-white/40 px-1.5 py-0.5 text-[10px] font-bold transition-colors hover:bg-white hover:text-black">
                {playbackSpeed}x
              </button>

              {/* Simple Pop-up Menu */}
              <div className="absolute right-0 bottom-full hidden flex-col overflow-hidden rounded border border-white/10 bg-black/90 shadow-xl group-hover/speed:flex">
                {[0.25, 0.5, 0.75, 1, 1.25, 1.5, 1.75, 2].map((speed) => (
                  <button
                    key={speed}
                    onClick={() => handleSpeedChange(speed)}
                    className={`hover:bg-accent px-4 py-2 text-xs ${playbackSpeed === speed ? "bg-accent/50 text-base" : ""}`}
                  >
                    {speed}x
                  </button>
                ))}
              </div>
            </div>
            {/*Wide Screen Button*/}
            <button
              onClick={() => {}}
              className="text-white hover:text-gray-300"
            >
              <svg
                xmlns="http://www.w3.org/2000/svg"
                width="24"
                height="24"
                viewBox="0 0 24 24"
                fill="none"
                stroke="currentColor"
                stroke-width="2"
                stroke-linecap="round"
                stroke-linejoin="round"
                className="icon icon-tabler icons-tabler-outline icon-tabler-viewport-wide"
              >
                <path stroke="none" d="M0 0h24v24H0z" fill="none" />
                <path d="M10 12h-7l3 -3" />
                <path d="M6 15l-3 -3" />
                <path d="M14 12h7l-3 -3" />
                <path d="M18 15l3 -3" />
                <path d="M3 6v-1a2 2 0 0 1 2 -2h14a2 2 0 0 1 2 2v1" />
                <path d="M3 18v1a2 2 0 0 0 2 2h14a2 2 0 0 0 2 -2v-1" />
              </svg>
            </button>

            {/* Fullscreen Button */}
            <button
              onClick={() => playerRef.current?.requestFullscreen()}
              className="text-white hover:text-gray-300"
            >
              <svg
                xmlns="http://www.w3.org/2000/svg"
                width="24"
                height="24"
                viewBox="0 0 24 24"
                fill="none"
                stroke="currentColor"
                stroke-width="2"
                stroke-linecap="round"
                stroke-linejoin="round"
                className="icon icon-tabler icons-tabler-outline icon-tabler-maximize"
              >
                <path stroke="none" d="M0 0h24v24H0z" fill="none" />
                <path d="M4 8v-2a2 2 0 0 1 2 -2h2" />
                <path d="M4 16v2a2 2 0 0 0 2 2h2" />
                <path d="M16 4h2a2 2 0 0 1 2 2v2" />
                <path d="M16 20h2a2 2 0 0 0 2 -2v-2" />
              </svg>
              {/*<i className="hn hn-expand-solid"></i>*/}
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Player;
