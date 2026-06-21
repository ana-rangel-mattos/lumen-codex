import { ErrorComponent, useParams, useSearch } from "@tanstack/react-router";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import {
  baseUrl,
  getLessonById,
  getLessonSubtitles,
  getLessonTextContent,
  updateLessonCompletion,
} from "../../services/courseApi.ts";
import DOMPurify from "dompurify";
import { LinkButton, Player, Spinner } from "../../components";

const Lesson = () => {
  // const navigate = useNavigate({ from: "/course/$courseId/$lessonId" });
  const queryClient = useQueryClient();

  const { lessonId, courseId } = useParams({
    from: "/course/$courseId/$lessonId",
  });

  const { prevLessonId, nextLessonId } = useSearch({
    from: "/course/$courseId/$lessonId",
  });

  const {
    isPending: isLoadingLesson,
    error: lessonError,
    data: lessonData,
  } = useQuery({
    queryKey: ["lesson", lessonId],
    queryFn: () => getLessonById(lessonId),
  });

  const {
    isPending: isLoadingSubtitles,
    error: subtitlesError,
    data: subtitlesData,
  } = useQuery({
    queryKey: ["subtitles", lessonId],
    queryFn: () => getLessonSubtitles(lessonId),
  });

  const {
    isPending: isLoadingContent,
    error: contentError,
    data: contentData,
  } = useQuery({
    queryKey: ["text", lessonId],
    queryFn: () => getLessonTextContent(lessonId),
  });

  const handleCompletionChange = (completion: boolean) => {
    completionMutation.mutate({ lessonId, completion });
  };

  const completionMutation = useMutation({
    mutationKey: ["lesson", lessonId],
    mutationFn: (obj: { lessonId: string; completion: boolean }) =>
      updateLessonCompletion(obj.lessonId, obj.completion),
    onSuccess: async () => {
      await queryClient.invalidateQueries({ queryKey: ["lesson"] });
    },
  });

  if (isLoadingLesson || isLoadingSubtitles || isLoadingContent) {
    return <Spinner />;
  }

  if (lessonError || subtitlesError || contentError) {
    return (
      <ErrorComponent error={lessonError || subtitlesError || contentError} />
    );
  }

  return (
    <div className="bg-secondary col-start-2 col-end-7 row-start-1 row-end-5">
      {lessonData?.lessonType == "video" && (
        <div className="mx-auto max-w-[100%]">
          <Player
            videoUrl={`${baseUrl}${lessonData?.streamPath}`}
            subtitles={subtitlesData}
            lessonName={lessonData.lessonName}
          />
        </div>
      )}

      {lessonData?.lessonType == "text" && (
        <div
          className="text-base-100 mx-auto mt-10 h-2/3 max-w-2/3 overflow-auto text-xl contain-content"
          dangerouslySetInnerHTML={{
            __html: DOMPurify.sanitize(contentData),
          }}
        />
      )}

      <div className="flex h-30 items-center justify-center space-x-5">
        <LinkButton
          disabled={!prevLessonId}
          to="/course/$courseId/$lessonId"
          params={{ lessonId: prevLessonId || "", courseId }}
          icon="hn hn-arrow-circle-left"
        >
          <span className="contents">Previous</span>
        </LinkButton>

        <LinkButton
          disabled={!nextLessonId}
          to="/course/$courseId/$lessonId"
          params={{ lessonId: nextLessonId || "", courseId }}
          icon="hn hn-arrow-circle-right"
        >
          <span className="contents">Next</span>
        </LinkButton>

        <fieldset className="fieldset bg-base-content text-base-100 border-accent rounded-box w-64 border-2 px-4 py-3 text-lg">
          <label className="label">
            <input
              type="checkbox"
              disabled={completionMutation.isPending}
              onChange={(e) => handleCompletionChange(e.target.checked)}
              checked={lessonData?.isCompleted}
              className="checkbox checkbox-sm checkbox-success"
            />
            {lessonData?.isCompleted
              ? "Complete"
              : completionMutation.isPending
                ? "Loading..."
                : "Mark as Complete"}
          </label>
        </fieldset>
      </div>
    </div>
  );
};

export default Lesson;
