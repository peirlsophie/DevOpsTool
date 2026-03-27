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
