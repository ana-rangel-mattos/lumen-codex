export const secondsToTime = (seconds?: number): string | null => {
  if (!seconds) return null;

  const date = new Date(seconds * 1000);

  return date.toISOString().substring(11, 19);
};

export const formatTime = (time: number): string => {
  if (isNaN(time)) return "00:00";

  const minutes = Math.floor(time / 60);
  const seconds = Math.floor(time % 60);

  return `${minutes.toString().padStart(2, "0")}:${seconds.toString().padStart(2, "0")}`;
};
