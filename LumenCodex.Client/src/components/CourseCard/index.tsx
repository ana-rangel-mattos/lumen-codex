import type { Course } from "../../types/course.ts";
import * as React from "react";
import { useState } from "react";
import { Button, Dialog, LinkButton } from "../index.ts";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { deleteCourse } from "../../services/courseApi.ts";

interface ICourseCardProps {
  course: Course;
}

const CourseCard: React.FC<ICourseCardProps> = ({ course }) => {
  const [isDialogOpen, setIsDialogOpen] = useState<boolean>(false);
  const queryClient = useQueryClient();

  const onDeleteCourse = () => {
    courseMutation.mutate(course.courseId);
    setIsDialogOpen(false);
  };

  const courseMutation = useMutation({
    mutationKey: ["courses", course.courseId],
    mutationFn: (courseId: string) => deleteCourse(courseId),
    onSuccess: async () => {
      await queryClient.invalidateQueries({ queryKey: ["courses"] });
    },
  });

  return (
    <li className="card bg-secondary relative w-96 shadow-sm">
      <Dialog
        isOpen={isDialogOpen}
        setIsOpen={setIsDialogOpen}
        dialogTitle="Delete Course?"
        content={`Are you sure you wish to delete ${course.courseName}? This action cannot be undone.`}
        onConfirm={onDeleteCourse}
      />

      <div className="card-body">
        <h3 className="text-base-100 pb-4 text-xl">{course.courseName}</h3>
        <div className="card-actions items-center justify-between">
          <Button
            buttonStyle="warning"
            size="xs"
            onClick={() => setIsDialogOpen((prev) => !prev)}
          >
            Delete
          </Button>
          <LinkButton
            to="/course/$courseId"
            params={{ courseId: course.courseId }}
            icon="hn hn-external-link"
          >
            <span className="contents">Go to course</span>
          </LinkButton>
        </div>
      </div>
    </li>
  );
};

export default CourseCard;
