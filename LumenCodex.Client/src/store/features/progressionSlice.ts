import { createSlice, type PayloadAction } from "@reduxjs/toolkit";

interface ProgressState {
  completedLessons: Record<string, boolean>;
}

const initialState: ProgressState = {
  completedLessons: {},
};

export const progressSlice = createSlice({
  name: "progress",
  initialState,
  reducers: {
    toggleLessonCompletion: (
      state,
      action: PayloadAction<{ lessonId: string; isCompleted: boolean }>,
    ) => {
      const { lessonId, isCompleted } = action.payload;

      if (isCompleted) {
        state.completedLessons[lessonId] = true;
      } else {
        delete state.completedLessons[lessonId];
      }
    },
    setInitialProgress: (state, action: PayloadAction<string[]>) => {
      action.payload.forEach((lessonId) => {
        state.completedLessons[lessonId] = true;
      });
    },
  },
});

export const { toggleLessonCompletion, setInitialProgress } =
  progressSlice.actions;

export default progressSlice.reducer;
