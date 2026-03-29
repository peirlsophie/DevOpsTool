function checkQuiz(quizId, correctAnswer) {
  const form = document.getElementById(quizId);
  const feedback = document.getElementById("feedback-" + quizId);
  const selected = form.querySelector(
    'input[name="' + form.elements[0].name + '"]:checked',
  );

  if (!selected) {
    feedback.innerHTML = "Please select an answer!";
    feedback.style.color = "orange";
    return;
  }

  if (selected.value === correctAnswer) {
    feedback.innerHTML = "Correct! Well done.";
    feedback.style.color = "#42b883";
  } else {
    feedback.innerHTML = "Wrong answer. Try again!";
    feedback.style.color = "#ff4757";
  }
}

function nextStep(stepNumber) {
  const steps = document.querySelectorAll(".step");

  steps.forEach((step) => {
    step.style.display = "none";
  });

  const currentStep = document.getElementById("step-" + stepNumber);
  if (currentStep) {
    currentStep.style.display = "block";
    window.scrollTo(0, 0);
  }
}
