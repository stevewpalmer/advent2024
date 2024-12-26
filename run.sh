#!/bin/bash
SOLUTION_DIR="$(pwd)"
for PROJECT_DIR in "$SOLUTION_DIR"/*; do
    if [ -d "$PROJECT_DIR" ]; then
        PROJECT_NAME=$(basename "$PROJECT_DIR")
        EXECUTABLE_PATH="$PROJECT_DIR/bin/Release/net8.0/$PROJECT_NAME"
        if [ -f "$EXECUTABLE_PATH" ]; then
            echo "Running project '$PROJECT_NAME'..."
            cd "$PROJECT_DIR" || exit
            time "$EXECUTABLE_PATH"
        else
            echo "Warning: Executable not found for project '$PROJECT_NAME' at '$EXECUTABLE_PATH'. Skipping."
        fi
    fi
done

